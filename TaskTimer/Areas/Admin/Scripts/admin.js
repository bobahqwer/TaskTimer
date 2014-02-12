function OpenDialogEl(msg, el, returnVal)
{
    if (returnVal != null)
        return returnVal;
    OpenDialogEl(msg, el);
    return false;
}

function OpenDialogEl(msg, el)
{
    var $myDialog = $('<div></div>')
    .html('Delete question?')
    .dialog({
        title: 'Conformation',
        autoOpen: false,
        width: 300,
        resizable: false,
        modal: true,
        position: { my: "left top", at: "left bottom", of: el },
        buttons: {
            "Delete": function () {
                $(this).dialog("close");
                el.click(msg, el, true);
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
                var inputText = $(elTds[i]).find('input[type="text"]');
                inputText.attr('disabled',true);
                inputText.val(textarea.html());
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
                var inputText = $(elTds[i]).find('input[type="text"]');
                inputText.attr('disabled', false);
                inputText.val(textarea.html());
            i++;
        });
    }
}

function CloseAllRowEditor(element, e) {

    e = e || window.event;
    var targ = e.target || e.srcElement;

    debugger;
    var table = $(element).closest('.tablesorter');

    var allEditRows = table.find('.textBoxEdit').filter(function (index) {
        return $(this).css("display") === "table-row";
    });
    allEditRows.each(function () {
        CloseRowEditor($(this), e);
    });
    return false;
}
