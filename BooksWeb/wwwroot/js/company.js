// Everything responsible for building the datatable is in this file 
var datatable = $('#datatable').DataTable();

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    console.log("shit")
    dataTable = $('#tblData').DataTable({
        // make ajax request ot load data
        "ajax": {
            // give request a path
            "url":"/Admin/Company/GetAll"
        },
        // once we received the request we have to parse the data (columns)
        "columns": [
            { "data":"name","width": "15%"},
            {"data":"streetAddress","width": "15%"},
            {"data":"city","width": "15%"},
            { "data": "state", "width": "15%" },
            // to see why we use category.name use inspect to see the api call and json returned.
            // youll notice an array full of KV pairs is returned for category
            { "data": "postalCode", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            // now we want to render html so we can add buttons
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Company/Upsert?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a onClick=Delete('/Admin/Company/Delete/${data}')
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					</div>
                        `
                },
                "width": "15%"
            }
            
        ]
    });
}

// pass the URl that we have to invoke to hit our endpoint inside the product controller
function Delete(url) {
    console.log("sheit")
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            // instead of firing a new sweet alert we have to make an ajax request to hit our endpoint
            if (result.isConfirmed) {
                console.log("confimred");
                $.ajax({
                    url: url,
                    type: 'DELETE',
                    success: function (data) {
                        if (data.success) {
                            console.log(datatable);
                            // if the delete was a success we want to reload the datatable
                            // remember we have the datatable being built using json above
                            datatable.ajax.reload();
                            // added toastr notification in _layout so we can use that
                            // use toastr success to dispaly message
                            toastr.success(data.message);
                        }
                        else {
                            // use toastr error to dispaly message
                            toastr.error(data.message);
                        }
                    }
                })
            }

            // blow is the code that came from sweet alert originally
            //Swal.fire(
            //    'Deleted!',
            //    'Your file has been deleted.',
            //    'success'
            //)
        }
    })
}