var _formElement;

function SystemRoleElmListForm_onLoad(formElement) {
    _formElement = formElement;

    var table = $('#SystemRoleElmListTable', _formElement);
    if (table.length > 0 && $('tr', table).length > 11) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch').each(function() { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }

    $('.BaseContainer').css('z-index', 0);
    $('#FunControllerID').val($('#FunControllerID').attr('originalvalue'));
    FunControllerID_onChange({ select: $('#FunControllerID') });
    $('#FunActionName').val($('#FunActionName').attr('originalvalue'));

    $('table.tblsearch #FunControllerID', _formElement).combobox({
        width: 180,
        isRemoveButton: true,
        onChange: FunControllerID_onChange
    });

    $('table.tblsearch #FunActionName', _formElement).combobox({
        width: 180,
        isRemoveButton: true
    });
}

function FunControllerID_onChange(event) {
    $('#FunActionName > option', _formElement).remove();
    $('#FunActionName', _formElement).combobox('SetSelected', '');
    $('#FunActionName').html('<option value=""></option>');

    if (event.select.val()) {
        var sysID = $('#SysID').val();
        $(systemInfo).filter(function(idx, el) {
            return el.Sys.Value === sysID;
        }).map(function(idx, el) {
            $(el.FunActionList).filter(function(idxFun, elFun) {
                return elFun.GroupID === sysID + '|' + event.select.val();
            }).map(function(idxFun, elFun) {
                $('#FunActionName').append('<option value="' + elFun.Value + '">' + elFun.Text + '</option>');
            });
        });
    }
    return true;
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    location.href = '/Sys/SystemRole';
}

function LinkFunElmDetail_onClick(srcElement,keys) {
    $.blockUI({ message: '' });

    $('#FunElmID').val(keys[1]);
    $('#SysID').val(keys[2]);
    $('#FunControllerID').val(keys[3]);
    $('#FunActionName').html('<option value="' + keys[4] +'"></option>');

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function AddButton_onClick(srcElement) {
    var number = Math.floor(100000 * Math.random()) + 100;

    var tr = document.createElement('tr');

    var delBtn = $(document.createElement('img')).attr({
        'border': '0',
        'id': 'DeleteRowButton',
        'onclick': '_ImageButton_onClick(this)',
        'src': deletebutton,
        'style': 'cursor:pointer'
    });

    var funControlCombobox = $(document.createElement('select')).attr({
        'id': 'SysRoleElmList[' + number + '].FunControllerID',
        'name': 'SysRoleElmList[' + number + '].FunControllerID'
    });

    var funActionCombobox = $(document.createElement('select')).attr({
        'id': 'SysRoleElmList[' + number + '].FunActionName',
        'name': 'SysRoleElmList[' + number + '].FunActionName'
    });

    var funElmIDCombobox = $(document.createElement('select')).attr({
        'id': 'SysRoleElmList[' + number + '].ElmID',
        'name': 'SysRoleElmList[' + number + '].ElmID'
    });

    var displayStsCombobox = $(document.createElement('select')).attr({
        'id': 'SysRoleElmList[' + number + '].DisplaySts',
        'name': 'SysRoleElmList[' + number + '].DisplaySts'
    });

    var isAdd = $(document.createElement('input')).attr({
        'id': 'SysRoleElmList[' + number + '].IsAdd',
        'name': 'SysRoleElmList[' + number + '].IsAdd',
        'value': 'Y',
        'type': 'hidden'
    });

    $('table[id=SystemRoleElmListTable]').append(
        $(tr).append($(document.createElement('td')).append(delBtn))
        .append($(document.createElement('td')).attr('data-th', window.JsMsg_Table_FunControllerID).append(funControlCombobox))
        .append($(document.createElement('td')).attr('data-th', window.JsMsg_Table_FunActionName).append(funActionCombobox))
        .append($(document.createElement('td')).attr('data-th', window.JsMsg_Table_FunElmID).append(funElmIDCombobox))
        .append($(document.createElement('td')).attr('data-th', window.JsMsg_Table_DisplaySts).append(displayStsCombobox))
        .append($(document.createElement('td')))
        .append($(document.createElement('td')).attr('data-th', window.JsMsg_Table_UpdUserNM).append(isAdd))
        .append($(document.createElement('td')))
    );

    _SetComboBox(tr);

    return true;
}

function _SetComboBox(tr) {
    var controller = $('select[name$=FunControllerID]', tr);
    var displaySts = $('select[name$=DisplaySts]', tr);

    $(controllerComboBox).each(function (idx, item) {
        $(controller).append('<option value="' + item.Key + '">' + item.Value + '</option>');
    });

    $(controller).combobox({
        width: 90,
        isRemoveButton: true,
        onChange: _GetSystemFunActionNameList,
        isChangeWidth: true
    });

    $('select[name$=FunActionName]', tr).combobox({
        width: 90,
        isRemoveButton: true,
        onChange: _GetSystemElmID,
        isChangeWidth: true
    });

    $('select[name$=ElmID]', tr).combobox({
        width: 90,
        isRemoveButton: true,
        isChangeWidth: true
    });

    $(displayStsComboBox).each(function (idx, item) {
        $(displaySts).append('<option value="' +item.Key + '">' + item.Value + '</option>');
    });

    $(displaySts).combobox({
        width: 40,
        isRemoveButton: true
    });
}

function DeleteRowButton_onClick(srcElement) {
    if ($(srcElement).closest('tr').find('input[name$=IsAdd]').val() !== 'Y') {
        var originalFunInfo = $(srcElement).closest('tr').find('input#OriginalFunInfo').val().split('|');
        var delTr = $(document.createElement('tr')).attr({ 'id': 'DelRow' }).css({ 'display': 'none' });

        var delfunController = $(document.createElement('input')).attr({
            'id': 'SysRoleElmList[100].FunControllerID',
            'name': 'SysRoleElmList[100].FunControllerID',
            'value': originalFunInfo[0],
            'type': 'hidden'
        });

        var delfunAction = $(document.createElement('input')).attr({
            'id': 'SysRoleElmList[100].FunActionName',
            'name': 'SysRoleElmList[100].FunActionName',
            'value': originalFunInfo[1],
            'type': 'hidden'
        });

        var delfunElm = $(document.createElement('input')).attr({
            'id': 'SysRoleElmList[100].ElmID',
            'name': 'SysRoleElmList[100].ElmID',
            'value': originalFunInfo[2],
            'type': 'hidden'
        });

        $('table[id=SystemRoleElmListTable] tr:eq(0)')
            .after(delTr.append($(document.createElement('td')).append(delfunController, delfunAction, delfunElm)));
    }

    $(srcElement).closest('tr').remove();

    return true;
}

function _GetSystemFunActionNameList(event) {
    var sysID = $('#SysID').val();
    var selectFunActionName = event.select.closest('tr').find('select[id$="FunActionName"]', _formElement);
    var selectElmID = event.select.closest('tr').find('select[id$="ElmID"]', _formElement);
    selectFunActionName.combobox('SetSelected', '');
    selectElmID.combobox('SetSelected', '');
    selectFunActionName.html('<option value=""></option>');
    selectElmID.html('<option value=""></option>');

    $(systemInfo).filter(function(idx, el) {
        return el.Sys.Value === sysID;
    }).map(function(idx, el) {
        $(el.FunActionList).filter(function(idx, el) {
            return el.GroupID === sysID + '|' + event.select.val();
        }).map(function(idx, item) {
            selectFunActionName.append('<option value="' + item.Value + '">' + item.Text + '</option>');
        });
    });

    return true;
}

function _GetSystemElmID(event) {
    var selectElmID = event.select.closest('tr').find('select[id$="ElmID"]', _formElement);
    selectElmID.combobox('SetSelected', '');
    selectElmID.html('<option value=""></option>');

    $.ajax({
        url: '/Sys/GetSystemElmID',
        type: 'POST',
        data: {
            sysID: $('#SysID').val(),
            controllerID: event.select.closest('tr').find('option:selected', $('select[name$="FuncontrollerID"]')).val(),
            actionName: event.select.val()
        },
        dataType: 'json',
        async: false,
        success: function(result) {
            if (result != null) {
                for (var idx = 0; idx < result.length; idx++) {
                    selectElmID.append('<option value="' + result[idx].Value + '">' + result[idx].Text + '</option>');
                }
            }
        },
        error: function(jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(window.JsMsg_GetSystemElmIDAndSystemElmNameList_Failure);
            _ShowJsErrMessageBox();
        }
    });

    return true;
}

function SaveButton_onClick(srcElement) {
    var result = _FormValidation() && _ValidateRequired() && _ValidateRoleFunElmData();

    if (result) {
        $('#ExecAction').val(_ActionTypeUpdate);
        _formElement.submit();
        return true;
    }

    _ShowJsErrMessageBox();
    return false;
}

function _ValidateRoleFunElmData(srcElement) {
    var result = true;
    var reNameIndex = 0;
    var allValArray = new Array(0);
    var table = $('table[id=SystemRoleElmListTable]');

    $(table).find('tr').each(function(index, el) {
        var object = $(el).find("input:hidden[name^=SysRoleElmList],select");

        if (object.length > 0) {
            $(object).each(function(idx, item) {
                $(item).attr('name', _reName($(this).attr('name'), reNameIndex));
                $(item).attr('id', _reName($(this).attr('id'), reNameIndex));
            });
            reNameIndex++;
        }
    });

    $(table).find('tr:not([id="DelRow"])').not(':eq(0)').each(function(index, tr) {
        allValArray.push($(tr).find('input#OriginalFunInfo,select').not(':eq(3)').map(function(idx, el) {
            return $(el).val();
        }).toArray().join('|'));
    });

    $(table).find('tr:not([id="DelRow"])').not(':eq(0)').each(function(firstIndex, first) {
        var firstVal = _GetFunElmVal(first);
        allValArray.splice(0, 1);

        if ($.inArray(firstVal, allValArray) >= 0 && firstVal !== '') {
            _AddJsErrMessage(String.format(window.JsMsg_TheSameData, firstIndex + 1, ($.inArray(firstVal, allValArray) + (firstIndex + 2))));
            result = false;
        }
    });

    return result;
}

function _ValidateRequired(srcElement) {
    var result = true;
    var table = $('table[id=SystemRoleElmListTable]');

    $(table).find('tr:not([id="DelRow"])').not(':eq(0)').each(function(index, tr) {
        $('select', tr).each(function(idx, el) {
            if ($(el).val() === '') {
                _AddJsErrMessage(String.format(window.JsMsg_RequiredAllDropDownMenu, (index + 1)));
                result = false;
                return false;
            }
        });
    });

    return result;
}

function _GetFunElmVal(srcElement) {
    var value = $('input#OriginalFunInfo', srcElement).val();

    if (value === undefined) {
        value = $(srcElement).closest('tr').find('select').not(':eq(3)').map(function(idx, el) {
            return $(el).val();
        }).toArray().join('|');
    }

    return value;
}