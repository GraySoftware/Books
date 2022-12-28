// Everything responsible for building the datatable is in this file 
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
            {"data":"price","width": "15%"},
            { "data": "author", "width": "15%" },
            // to see why we use category.name use inspect to see the api call and json returned.
            // youll notice an array full of KV pairs is returned for category
            { "data": "category.name", "width": "15%" },
            // now we want to render html so we can add buttons
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/Admin/Product/Upsert?id=${data}"
                           class="btn btn-primary"><i class="bi bi-pen"></i>Edit</a>
                            <a "
                           class="btn btn-danger"><i class="bi bi-pen"></i>Delete</a>
                        </div>
                        `
                }

            }
            
        ]
    });
}