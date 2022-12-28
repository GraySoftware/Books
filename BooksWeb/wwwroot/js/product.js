var datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        // make ajax request ot load data
        "ajax": {
            // give request a path
            "url":"/Admin/Product/GetAll"
        },
        // once we received the request we have to parse the data (columns)
        "columns": [
            { "data":"title","width": "15%"},
            {"data":"isbn","width": "15%"},
            {"data":"price","width": "15%"}
        ]
    });
}