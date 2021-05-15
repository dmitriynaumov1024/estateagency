function addBookmark() {
    $.ajax({
        type: "POST",
        url: "/ajax/addbookmark",
        data: { id: objectID },
        success: function (data) {
            if (data==true) setBookmarkButton(true);
        }
    });
}

function delBookmark() {
    $.ajax({
        type: "POST",
        url: "/ajax/delbookmark",
        data: { id: objectID },
        success: function (data) {
            if (data == true) setBookmarkButton(false);
        }
    });
}

function setBookmarkButton(i) {
    if (i) {
        $("#bookmark").html("У закладках.");
        $("#bookmark").click(delBookmark);
    } else {
        $("#bookmark").html("Додати в закладки");
        $("#bookmark").click(addBookmark);
    }
}

function page_main() {
    if (currentObjectBookmark!=undefined) setBookmarkButton(currentObjectBookmark);
}