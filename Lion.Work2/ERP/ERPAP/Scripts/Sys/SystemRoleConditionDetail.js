var _formElement;

function SystemRoleConditionDetailForm_onLoad(formElement) {
    _formElement = formElement;
    ConditionType_onClick();
    $('#QueryBuilderBox').queryBuilder({ modelName: 'SystemRoleConditionGroupRule', filters: filters, rules: rules });
}

function ConditionType_onClick(srcElement) {
    if ($('#ConditionType:checked').val() === 'AccordingCondition') {
        $('div #QueryBuilderBox').show();
    } else {
        $('div #QueryBuilderBox').hide();
    }
}

function CancelButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}

function AddRuleButton_onClick(srcElement) {
    var ul = $('>ul', $(srcElement).closest('dt').next('dd'));
    ul.append($('li:eq(0)', ul).clone());

    return false;
}

function AddGroupButton_onClick(srcElement) {
    var ul = $('>ul', $(srcElement).closest('dt').next('dd'));
    ul.append($('dl', ul).clone());

    return false;
}

function DeleteButton_onClick(srcElement) {
    _alert('MessageConfirm');
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

function ConfirmOKButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeDelete);
    _formElement.submit();

    _btnUnblockUI(this, false);
}

function DeleteRuleButton_onClick(srcElement) {
    $(srcElement).closest('li').remove();
    return false;
}

function DeleteGroupButton_onClick(srcElement) {
    $(srcElement).closest('dl').remove();
    
    return false;
}

function UpdateButton_onClick(srcElement) {
    if (_SystemRoleConditionDetailFormValidation() && _FormValidation()) {
        $.ajax({
            url: '/Sys/GetRoleConditionSyntax',
            type: 'POST',
            data: $(_formElement).serialize(),
            dataType: 'json',
            async: false,
            success: function(result) {
                var newline = result.match(/\n/g);
                var rows = (newline !== null && newline !== undefined) ?
                    (newline.length > 10 ? 10 : newline.length) + 1 :
                    1;

                $('#Syntax').val(result);
                $('div#dialog_Confirm textarea').attr('rows', rows);
                $.blockUI({ message: $('#dialog_Confirm') });
            }
        });
        return true;
    }
    _ShowJsErrMessageBox();
    return false;
}

function ConditionOKButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeUpdate);
    _formElement.submit();
}

function ConditionCancelButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

function AddButton_onClick(srcElement) {
    if (_SystemRoleConditionDetailFormValidation() && _FormValidation()) {
        $('#ExecAction').val(_ActionTypeAdd);
        return true;
    }
    _ShowJsErrMessageBox();
    return false;
}

function _SystemRoleConditionDetailFormValidation() {

    var result = true;

    if ($('#SysRoleList:checked').length === 0) {
        _AddJsErrMessage(JsMsg_IsValidSysRoleRequired_Failure);
        result = false;
    }
    if ($('#ConditionType:checked').val() === 'AccordingCondition') {
        if ($('select[name ^= SystemRoleConditionGroupRule][name $= ID]', $('#QueryBuilderBox')).size() === 0 ||
            $.grep($('select[name ^= SystemRoleConditionGroupRule][name $= ID]', $('#QueryBuilderBox')), function(item) { return item.value !== '' }).length === 0) {
            _AddJsErrMessage(JsMsg_IsValidSysRoleRulesRequired_Failure);
            result = false;
        }
    }

    return result;
}