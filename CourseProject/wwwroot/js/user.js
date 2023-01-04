var dataTable;

$(document).ready(function () {
    loadDataTable("")
});

function loadDataTable(url) {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url":"/user/GetUserList"
        },
        "columns": [
            { "data": "id", "width": "20%" },
            { "data": "fullName", "width": "25%" },
            { "data": "email", "width": "25%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="text-center">
                        <a href="User/Details/${data}" class="btn btn-success text-white" style="cursor:pointer">
                            <i class="far fa-eye"></i>
                        </a>
                    </div>
                    `;
                },
                "width":"5%"
                }
            ]
        })
}