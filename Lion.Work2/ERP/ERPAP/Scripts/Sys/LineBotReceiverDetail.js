var _formElement;

function LineBotReceiverDetailForm_onLoad(formElement) {
    _formElement = formElement;
}

function CancelButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeSelect);
    return true;
}

function UpdateButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (result) {
        $.blockUI({ message: '' });
        return true;
    }
    return false;
}

function LeaveButton_onClick(srcElement) {
    var dialog = $(document.createElement('div'));

    dialog.dialog({
        modal: true,
        resizable: false,
        title: JsMsg_LineBotIsLeave,
        height: 150,
        width: 200,
        close: function() {
            dialog.dialog('destroy');
            dialog.remove();
        },
        buttons: {
            change: {
                "class": 'btn',
                text: JsMsg_Confirm,
                click: function() {
                    $.blockUI({ message: '' });
                    $('input[name = ExecAction]').val(_ActionTypeDelete);
                    $(_formElement).attr('action', '/Sys/LineBotReceiverDetail');
                    _formElement.submit();
                }
            },
            close: {
                "class": 'btn',
                text: JsMsg_Close,
                click: function() {
                    dialog.dialog('close');
                }
            }
        }
    });

    dialog.html($(document.createElement('div')).html(JsMsg_LineBotSureToLeave));

    return true;
}