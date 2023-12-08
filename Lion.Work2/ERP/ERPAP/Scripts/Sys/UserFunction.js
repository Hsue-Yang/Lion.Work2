var _formElement;

function UserFunctionForm_onLoad(formElement) {
    _formElement = formElement;

    $('select[name$=SysID][name!=QuerySysID]', _formElement).combobox(
        {
            width: 200,
            isRemoveButton: true,
            onChange: FunSysID_onChange
        }
    );

    $('select[name$=FunControllerID]', _formElement).combobox(
        {
            width: 200,
            isRemoveButton: true,
            onChange: FunControllerID_onChange
        }
    );

    $('select[name$=FunActionName]', _formElement).combobox(
        {
            width: 200,
            isRemoveButton: true
        }
    );

    $('select[name=QuerySysID]', _formElement).combobox(
        {
            width: 200,
            isRemoveButton: true,
            onChange: QuerySysID_onChange
        }
    );

    $('.ui-combobox>span').css({ 'margin-top': 0 });

    return true;
}

function FunSysID_onChange(event) {
    var tr = $(event.select).closest('tr');

    var funController = $('select[name$=FunControllerID]', tr);
    var funAction = $('select[name$=FunActionName]', tr);

    var funControllerObj = $(funController).combobox('Selected');
    funControllerObj.input.val('');
    $('option', funControllerObj.select).remove();

    var funActionObj = $(funAction).combobox('Selected');
    funActionObj.input.val('');
    $('option', funActionObj.select).remove();
    
    if (event.select.val() !== '') {
        $.ajax({
            url: '/Sys/GetSystemFunControllerIDList',
            type: 'POST',
            data: { sysID: event.select.val() },
            dataType: 'json',
            async: false,
            success: function (result) {
                if (result) {
                    for (var i = 0; i < result.length; i++) {
                        funControllerObj.select.append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                    }
                }
            },
            error: function () {
                _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
                _ShowJsErrMessageBox();
            }
        });
    }
}

function FunControllerID_onChange(event) {
    var tr = $(event.select).closest('tr');

    var sys = $('select[name$=SysID]', tr);
    var funAction = $('select[name$=FunActionName]', tr);
    
    var funActionObj = $(funAction).combobox('Selected');
    funActionObj.input.val('');
    $('option', funActionObj.select).remove();

    if (event.select.val() !== '') {
        $.ajax({
            url: '/Sys/GetSystemFunActionNameList',
            type: 'POST',
            data: { sysID: sys.val(), funControllerID: event.select.val() },
            dataType: 'json',
            async: false,
            success: function(result) {
                if (result) {
                    for (var i = 0; i < result.length; i++) {
                        funActionObj.select.append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                    }
                }
            },
            error: function() {
                _AddJsErrMessage(JsMsg_GetSystemFunActionNameList);
                _ShowJsErrMessageBox();
            }
        });
    }
}

function QuerySysID_onChange(event) {
    $('tr> td input[id$="SysID"],select[id$="SysID"]', $('table#SystemFunTable')).closest('tr').show();
    if (event.select.val() !== '') {
        $('tr> td input[id$="SysID"],select[id$="SysID"]', $('table#SystemFunTable')).each(function(idx, el) {
                if ($(el).val() !== '' && $(el).val() !== event.select.val()) {
                    $(el).closest('tr').hide();
                }
            }
        );
    }
}

//----Tab----//
function UserRoleFunDetail_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'UserRoleFunDetail');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

function UserPurview_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'UserPurview');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

//----Button----//
function UpdateButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    $('#ExecAction').val(_ActionTypeQuery);
    _formElement.submit();
}

function AddRowButton_onClick(srcElement) {
    _addTR('IsProcess');

    var tr = $('#SystemFunTable tr').last().show();

    tr.find('span.ui-combobox').remove();
    $('select[name$=FunControllerID]', tr).find('option').remove();
    $('select[name$=FunActionName]', tr).find('option').remove();

    $('select[name$=SysID]', tr).combobox(
        {
            width: 200,
            isRemoveButton: true,
            onChange: FunSysID_onChange
        }
    );

    $('select[name$=FunControllerID]', tr).combobox(
        {
            width: 200,
            isRemoveButton: true,
            onChange: FunControllerID_onChange
        }
    );

    $('select[name$=FunActionName]', tr).combobox(
        {
            width: 200,
            isRemoveButton: true
        }
    );

    $('.ui-combobox>span', tr).css({ 'margin-top': 0 });
}

function DeleteRowButton_onClick(srcElement) {
    var objComboBox = $('select[name*=\'.SysID\']');
    var objTR = _GetParentElementByTag(srcElement, 'TR');

    if (objComboBox.length == 1) {
        $('select[name*=\'.SysID\']').val('');
        $('select[name*=\'.FunControllerID\']').val('');
        $('select[name*=\'.FunActionName\']').val('');
    }
    else {
        $(objTR).find('input[id=IsProcess]').prop('checked', true);
    }

    _deleteTR('IsProcess');
}

function DeleteDataRowButton_onClick(srcElement) {
    var objTR = _GetParentElementByTag(srcElement, 'TR');

    $(objTR).find('input[id=IsProcess]').prop('checked', true);

    _deleteTR('IsProcess');
}