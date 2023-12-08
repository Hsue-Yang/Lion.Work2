var _formElement;

function SystemRoleFunListForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#SystemRoleFunListTable', _formElement);

    if (table.length > 0 && table.find('tr').length > 11) {
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch').each(function() { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }

    $('.BaseContainer').css('z-index', 0);

    $('#QueryFunControllerID', $('table.tblsearch', _formElement)).combobox({
        width: 180,
        isRemoveButton: true
    });

    return true;
}

function SelectButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);

    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function LinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#FunControllerID').val(keys[2]);
    $('#FunActionName').val(keys[3]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function Clean_HiddenValue() {
    $('#SysID').val('');
    $('#FunControllerID').val('');
    $('#FunActionName').val('');
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    location.href = '/Sys/SystemRole';
}

function AddButton_onClick(srcElement) {
    _CreateTr();

    return true;
}

function _CreateTr(srcElement) {
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
        'id': 'SysRoleFunList[' + number + '].FunControllerID',
        'name': 'SysRoleFunList[' + number + '].FunControllerID'
    }).append('<option value=""></option>');

    var funActionCombobox = $(document.createElement('select')).attr({
        'id': 'SysRoleFunList[' + number + '].FunActionName',
        'name': 'SysRoleFunList[' + number + '].FunActionName'
    });

    var isAdd = $(document.createElement('input')).attr({
        'id': 'SysRoleFunList[' + number + '].IsAdd',
        'name': 'SysRoleFunList[' + number + '].IsAdd',
        'value': 'Y',
        'type': 'hidden'
    });

    $('table[id=SystemRoleFunListTable]').append(
        $(tr).append($(document.createElement('td')).append(delBtn))
        .append($(document.createElement('td')))
        .append($(document.createElement('td')))
        .append($(document.createElement('td')).append(funControlCombobox))
        .append($(document.createElement('td')).append(funActionCombobox))
        .append($(document.createElement('td')).append(isAdd))
        .append($(document.createElement('td'))));

    _SetComboBox(tr);
}

function _SetComboBox(tr) {
    var controller = $('select[name$=FunControllerID]', tr);

    $(systemInfo).filter(function(idx, el) {
        return el.Sys.Value === $('#SysID').val();
    }).map(function(idx, el) {
        $(el.FunControllerList).map(function(idx, el) { $(controller).append('<option value="' + el.Value + '">' + el.Text + '</option>'); });
    });

    $(controller).combobox({
        width: 80,
        isRemoveButton: true,
        onChange: _GetSystemFunActionNameList,
        isChangeWidth: true
    });

    $('select[name$=FunActionName]', tr).combobox({
        width: 180,
        isRemoveButton: true,
        isChangeWidth: true
    });
}

function DeleteRowButton_onClick(srcElement) {
    if ($(srcElement).closest('tr').find('input[name$=IsAdd]').val() !== 'Y') {
        var originalFunInfo = $(srcElement).closest('tr').find('input#OriginalFunInfo').val().split('|');
        var delTr = $(document.createElement('tr')).attr({ 'id': 'DelRow' }).css({ 'display': 'none' });

        var delfunController = $(document.createElement('input')).attr({
            'id': 'SysRoleFunList[100].FunControllerID',
            'name': 'SysRoleFunList[100].FunControllerID',
            'value': originalFunInfo[0],
            'type': 'hidden'
        });

        var delfunAction = $(document.createElement('input')).attr({
            'id': 'SysRoleFunList[100].FunActionName',
            'name': 'SysRoleFunList[100].FunActionName',
            'value': originalFunInfo[1],
            'type': 'hidden'
        });

        $('table[id=SystemRoleFunListTable] tr:eq(0)')
            .after(delTr.append($(document.createElement('td')).append(delfunController, delfunAction)));
    }

    $(srcElement).closest('tr').remove();

    return true;
}

function SaveButton_onClick(srcElement) {
    var result = _FormValidation() && _ValidateRequired() && _ValidateRoleFunData();

    if (result) {
        $('#ExecAction').val(_ActionTypeUpdate);
        _formElement.submit();
        return true;
    }

    _ShowJsErrMessageBox();
    return false;
}

function _ValidateRoleFunData(srcElement) {
    var result = true;
    var reNameIndex = 0;
    var allValArray = new Array(0);

    var table = $('table[id=SystemRoleFunListTable]');

    $(table).find('tr').each(function(index, el) {
        var object = $(el).find("input:hidden[name^=SysRoleFunList],select");

        if (object.length > 0) {
            $(object).each(function(idx, item) {
                $(item).attr('name', _reName($(this).attr('name'), reNameIndex));
                $(item).attr('id', _reName($(this).attr('id'), reNameIndex));
            });
            reNameIndex++;
        }
    });

    $(table).find('tr:not([id="DelRow"])').not(':eq(0)').each(function(index, tr) {
        allValArray.push($(tr).find('input#OriginalFunInfo,select').map(function(idx, el) {
            return $(el).val();
        }).toArray().join('|'));
    });

    $(table).find('tr:not([id="DelRow"])').not(':eq(0)').each(function(firstIndex, first) {
        var firstVal = _GetFunVal(first);
        allValArray.splice(0, 1);

        if ($.inArray(firstVal, allValArray) >= 0) {
            _AddJsErrMessage(String.format(window.JsMsg_TheSameData, firstIndex + 1, ($.inArray(firstVal, allValArray) + (firstIndex + 2))));
            result = false;
        }
    });

    return result;
}

function _GetFunVal(srcElement) {
    var value = $('input#OriginalFunInfo', srcElement).val();

    if (value === undefined) {
        value = $(srcElement).closest('tr').find('select').map(function(idx, el) {
            return $(el).val();
        }).toArray().join('|');
    }

    return value;
}

function _ValidateRequired(srcElement) {
    var result = true;
    var table = $('table[id=SystemRoleFunListTable]');

    $(table).find('tr:not([id="DelRow"])').not(':eq(0)').each(function(index, tr) {
        $('select', tr).each(function(idx, el) {
            if ($(el).val() === '') {
                _AddJsErrMessage(String.format(window.JsMsg_RequiredControllerActionNM, (index + 1)));
                result = false;
                return false;
            }
        });
    });

    return result;
}

function _GetSystemFunActionNameList(event) {
    var action = event.select.closest('tr').find('select[id$="FunActionName"]', _formElement);
    $(action).combobox('SetSelected', '');
    $(action).html('<option value=""></option>');

    $(systemInfo).filter(function(idx, el) {
        return el.Sys.Value === $('#SysID').val();
    }).map(function(idx, el) {
        $(el.FunActionList).filter(function (idx, el) {
            return el.GroupID === $('#SysID').val() + '|' + event.select.val();
        }).map(function(idx, item) {
            $(action).append('<option value="' + item.Value + '">' + item.Text + '</option>');
        });
    });

    return true;
}