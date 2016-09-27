/// <reference path="jquery-1.9.1.intellisense.js" />

function load(url) {
    $.getJSON(url).done(render);
}

function render(response) {
    for (var i = 0; i < response.length; i++) {
        var comment = response[i];

        $("#comments").append(
            '<div class="folder-separator comment">'
                + comment.body
                + '<div class="author">'
                    + '<div class="left">'
                        + 'Commented on '
                        + comment.updated_at.substr(0, 10)
                        + ' by '
                        + '<a target="_blank" rel="nofollow" href="' + comment.user.html_url + '">'
                            + comment.user.login
                        + '</a>'
                    + '</div>'
                    + '<div class="clear"></div>'
                + '</div>'
            + '</div>'
        );
    }
}