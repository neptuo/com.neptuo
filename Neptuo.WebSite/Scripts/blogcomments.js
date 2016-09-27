/// <reference path="jquery-1.9.1.intellisense.js" />

function load(url) {
    if ('fetch' in window) {
        fetch(url).then(parseJson);
    } else {
        document.getElementById("comments").innerHTML = ''
            + '<div class="folder-separator comment">'
                + 'As your browser doesn\'t support fetch API, please visit the link to inserting comments also to display them.'
            + '</div>'
        ;
    }
}

function parseJson(response) {
    response.json().then(render);
}

function render(response) {
    var html = '';
    for (var i = 0; i < response.length; i++) {
        var comment = response[i];

        html += ''
            + '<div class="folder-separator comment">'
                + '<div class="left">'
                    + '<a target="_blank" rel="nofollow" href="' + comment.user.html_url + '" title="' + comment.user.login + '">'
                        + '<img src="' + comment.user.avatar_url + '" />'
                    + '</a>'
                + '</div>'
                + comment.body.replace(/\r\n/g, '<br />').replace(/\n/g, '<br />')
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
        ;
    }

    document.getElementById("comments").innerHTML = html;
}