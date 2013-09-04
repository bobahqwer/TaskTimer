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

