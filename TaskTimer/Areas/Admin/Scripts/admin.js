function OpenDialog(msg)
{
    //$('#OpenDialogDiv').dialog(
    //    {
    //        title: 'Conformation',
    //        autoOpen: false,
    //        width: 300,
    //        resizable: false,
    //        modal: true,
    //        buttons: {
    //            "Delete": function () {
    //                $.ajax({
    //                    url: '/FAQ/Delete',
    //                    type: 'POST',
    //                    data: { id: id },
    //                    success: function (result) {
    //                        element.toggle("slow");
    //                    }
    //                });
    //                $(this).dialog("close");
    //            },
    //            "Cancel": function () {
    //                $(this).dialog("close");
    //            }
    //        },
    //    }
    //    );
    var $myDialog = $('<div></div>')
    .html('Delete question?')
    .dialog({
        title: 'Conformation',
        autoOpen: false,
        width: 300,
        resizable: false,
        modal: true,
        buttons: {
            "Delete": function () {
                //$.ajax({
                //    url: '/FAQ/Delete',
                //    type: 'POST',
                //    data: { id: id },
                //    success: function (result) {
                //        element.toggle("slow");
                //    }
                //});
                $(this).dialog("close");
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        },
    });
    $myDialog.dialog('open');
}


//toggle nicEditors areas
function OpenRowEditor(element, e) {
    e = e || window.event;
    var targ = e.target || e.srcElement;
    if (targ.className.indexOf("TextAreaEditor") != -1) {
        var el = $(element);
        var elTds = el.find('td');
        var editorRow = el.next('tr').toggle('slow');
        var i = 0;
        editorRow.find('td').each(function () {
            //var textarea = $(this).find('textarea');
            var textarea = $(this).find('.nicEdit-main');
            if (textarea.length > 0) {
                $(elTds[i]).find('input[type="text"]').val(textarea.html());
                $(this).css('cursor', 'default');
            }
            i++;
        });
    }
}
function CloseRowEditor(element, e) {
    e = e || window.event;
    var targ = e.target || e.srcElement;
    if (targ.tagName == "TD") {
        var el = $(element);
        var elTds = el.prev('tr').find('td');
        el.toggle('slow');
        var i = 0;
        el.find('td').each(function () {
            //var textarea = $(this).find('textarea');
            var textarea = $(this).find('.nicEdit-main');
            if (textarea.length > 0)
                $(elTds[i]).find('input[type="text"]').val(textarea.html());
            i++;
        });
    }
}

