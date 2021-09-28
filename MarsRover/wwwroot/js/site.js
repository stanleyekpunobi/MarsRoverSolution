// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const apiUrl = 'https://localhost:44367/api/';
let counter = 1;

$('#navigatebtn').on('click', function () {

    $("#plateautable tbody tr").empty();

    let requestObject = { plateauCordinates:'', rovers:[], plateaudID:0, plateauScreenShot:'', plateauResult:'' };

    $('.text-danger').remove();

    let upperBounds = $('#upperrightplateau').val();

    let spaceCheck = /\s/;

    let upperBoundsReg = /(\s*[0-9]+){2}/;

    if (!upperBoundsReg.test(upperBounds)) {
        $('#upperrightplateau').after('<span class="text-danger">Maximum 3 characters eg 5 5</span>');
        return;
    }

    if (!spaceCheck.test(upperBounds)) {
        $('#upperrightplateau').after('<span class="text-danger">Please add space in plateau coordinate</span>');
        return;
    }

    let roveerRows = document.querySelectorAll(".rover-row");

    roveerRows.forEach(ele => {
        let position = ele.querySelector('.roverposition');
        let instruction = ele.querySelector('.roverinstruction');
        requestObject.rovers.push({ currentPosition: position.value, instructions: instruction.value })
    })

    requestObject.plateauCordinates = $('#upperrightplateau').val()

    let cordinates = requestObject.plateauCordinates.split(' ');

    let body = $('.grid-canvas');
    for (var i = 1; i <= cordinates[0]; i++) {
        let row = $('<tr></tr>');
        for (col = 1; col <= cordinates[1]; col++) {
            row.append(`<td data-conf=(${i},${col})></td>`);
        }
        body.append(row);
    }

    $('body').loading({
        stoppable: false
    });

    $.ajax({
        type: "POST",
        url: `${apiUrl}Plateau/MoveRover`,
        data: JSON.stringify(requestObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data, status, xhr) {
            $('#result').text(data);
            $('body').loading('stop');
            let dataSplit = data.split(' ');
            requestObject.plateauResult = data;
            let tableTds = $("#plateautable td");
            console.log(tableTds);
            for (var i = 0; i < dataSplit.length; i++) {
                
                if (dataSplit[i].match(/[a-z]/i)) {
                    let cordinates = `(${dataSplit[i - 2]},${dataSplit[i - 1]})`;
                    console.log(cordinates)
                    for (var d = 0; d < tableTds.length; d++) {
                        let att = tableTds[d].dataset.conf;
                        if (att === cordinates) {
                            tableTds[d].style.backgroundColor = "red";
                            tableTds[d].style.color = "white";
                            tableTds[d].style.textAlign = "center";
                            if (dataSplit[i] === "S") {
                                tableTds[d].innerHTML = '&#8595;'
                            }
                            else if (dataSplit[i] === "N") {
                                tableTds[d].innerHTML = '&#8593;'

                            }
                            else if (dataSplit[i] === "W") {
                                tableTds[d].innerHTML = '&#8592;'

                            }
                            else if (dataSplit[i] === "E") {
                                tableTds[d].innerHTML = '&#8594;'

                            }
                        }
                    }
                }
            }
           

            html2canvas(document.querySelector("#plateauCard")).then(canvas => {
                requestObject.plateauScreenShot = canvas.toDataURL();
                requestObject.plateauScreenShot = requestObject.plateauScreenShot.replace(/^data:image\/(png|jpg);base64,/, "");
                $.ajax({
                    type: "POST",
                    url: `${apiUrl}Plateau/CreateHistory`,
                    data: JSON.stringify(requestObject),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data, status, xhr) {

                    },
                    error: function (jqXhr, textStatus, errorMessage) {
                        console.log(textStatus);
                        console.log(errorMessage);
                        $('body').loading('stop');
                        Swal.fire({
                            confirmButtonColor: '#1b6ec2',
                            text: 'Unable to save history'
                        })

                    }
                });
            });

        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(textStatus);
            console.log(errorMessage);
            $('body').loading('stop');
            Swal.fire({
                confirmButtonColor: '#1b6ec2',
                text: 'Unable to navigate rover, please try again!'
            })
        }
    });

    

});

$('#addroverbtn').on('click', function () {

    counter++;

    if (counter >= 6) {
        Swal.fire({
            confirmButtonColor: '#1b6ec2',
            text: 'Only 5 rovers are allowed'
        })
        return;
    }

    let appendHtml = `<div class="rover-row">
                        <div class="form-row">
                            <div class="form-group col-md-5">
                                <label for="roverposition">Rover Start Position</label>
                                <input type="text" id='roverposition${counter}' class="roverposition form-control" placeholder="1 3 N">
                            </div>
                            <div class="form-group col-md-5">
                                <label for="instructions">Instructions</label>
                                <input type="text" id='instructions${counter}' class="roverinstruction form-control" placeholder="LLLMM">
                            </div>
                            <div class="form-group col-md-2 mt-2">
                                <label for="instructions"></label>
                                <button type="button" id='removeroverbtn${counter}' class="btn btn-sm btn-danger">Delete</button>
                            </div>
                        </div>
                        </div>`;

    $('#rovers').append(appendHtml);

    $(`#removeroverbtn${counter}`).on('click', function () {       
            $(this).parents("div.rover-row:first").remove();
    });
    
});
