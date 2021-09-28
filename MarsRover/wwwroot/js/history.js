$(document).ready(function () {

    $('#historytable').DataTable({
        ajax: {
            url: `${apiUrl}Plateau/GetAllRoute`,
            dataSrc: ""
        },
         columns: [
             { "data": "plateauDimension" },
             { "data": "routeResult" },
             { "data": null, defaultContent: `<button class='btn btn-primary'>View Details</button>` }
         
        ],
        select:true
    });

    let historyTable = $('#historytable').DataTable();

    $('#historytable tbody').on('click', 'tr', function () {
        console.log(historyTable.row(this).data());
        let modalText = 'Instructions: ';
        let dataSplit = historyTable.row(this).data().routeHistoryKey.split('-');

        for (var i = 0; i < dataSplit.length; i++) {
            modalText += `${dataSplit[i]} \n`
        }

        Swal.fire({
            confirmButtonColor: '#1b6ec2',
            text: modalText,
            imageUrl: historyTable.row(this).data().snapShotUrl,
            imageWidth: 400,
            imageHeight: 200,
            imageAlt: 'rover',
        });
    });

});