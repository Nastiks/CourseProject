var comments;

$(document).ready(function () {
    loadComments();
    setInterval(loadComments, 1000)
});

function loadComments(url) {
    $.get("/home/GetComments/" + window.location.href.substr(window.location.href.lastIndexOf('/') + 1), function (data) {
        var str = "";
        if (data.data.length > 0) {
            for (var i = 0; i < data.data.length; i++) {
                let author = data.data[i].author;
                let text = data.data[i].text;
                let likes = data.data[i].likes;
                let row = `<li class="list-group-item d-flex justify-content-between align-items-start" style="background: #FAFAD2">
                                <div class="ms-2 me-auto">
                                    <div><strong>${author}</strong></div>
                                    ${text}
                                </div>
                                <span class="badge bg-primary rounded-pill">${likes}</span>
                            </li>`;
                str += row;
            }
        }
        else {
            str = "<p>Oops...There are no comments on this review yet</p>";
        }        
        $("#commentsContainer").html(str);
    });   
}