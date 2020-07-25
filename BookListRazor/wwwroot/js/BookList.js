var dataTable;
$(document).ready(function () {
    loadData();
})

function loadData() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/book",
            "type": "GET",
            "dataType":"json"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "name", "width": "25%" },
            { "data": "author", "width": "25%" },
            { "data": "isbn", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/BookList/Upsert?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px'>Edit</a>
                                <a onclick="Delete('/api/book?id=${data}')" class='btn btn-danger text-white' style='cursor:pointer; width:70px'/>Delete</a>
                            </div>`
                }, "width":"20%"
            }
        ],
        "language": {
            "emptyTable":"Not data found"
        },
        "width":"100%"
    })
}

function Delete(url) {
    swal({
        title: "Are you sure want delete?",
        text: "Once deleted, you will not be able to recover!",
        icon: "warning",
        buttons:true,
        dangerMode: true
    }).then(isYes => {
        if (isYes) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message)
                        dataTable.ajax.reload()
                    } else {
                        toastr.error(data.message)
                    }
                }
            });
        }
    });
}