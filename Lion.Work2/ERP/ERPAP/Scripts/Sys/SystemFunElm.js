var _formElement;

function SystemFunElmForm_onLoad(formElement) {
    _formElement = formElement;

    var table = $('#SystemFunElmTable', _formElement);
    if (table.length > 0 && $('tr', table).length > 2) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch').each(function() { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px' });
    }

    $('.BaseContainer').css('z-index', 0);

    $('#SysID').val($('#SysID').attr('originalvalue'));
    SysID_onChange({ select: $('#SysID') });
    $('#FunControllerID').val($('#FunControllerID').attr('originalvalue'));
    FunControllerID_onChange({ select: $('#FunControllerID') });
    $('#FunActionName').val($('#FunActionName').attr('originalvalue'));

    $('table.tblsearch #SysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: SysID_onChange
    });

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

function LinkFunElmDetail_onClick(srcElement, key) {
    $.blockUI({ message: '' });
    var actionObj = $('#FunActionName > option', _formElement);
    if (actionObj.length === 1 && actionObj.val() === '') {
        $('#FunActionName', _formElement).append('<option value="' + key[4] + '"></option>');
    }

    $('#FunElmID').val(key[1]);
    $('#SysID').val(key[2]);
    $('#FunControllerID').val(key[3]);
    $('#FunActionName').val(key[4]);
    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function SysID_onChange(event) {
    var sysValue = event.select.val();
    $('#FunControllerID > option', _formElement).remove();
    $('#FunControllerID', _formElement).combobox('SetSelected', '');
    $('#FunControllerID').html('<option value=""></option>');

    if (sysValue !== '') {
        $(systemInfo).filter(function(idx, el) {
            return el.Sys.Value === sysValue;
        }).map(function(idx, el) {
            $(el.FunControllerList).map(function(idxCon, elCon) {
                $('#FunControllerID').append('<option value="' + elCon.Value + '">' + elCon.Text + '</option>');
            });
        });
    }

    FunControllerID_onChange();

    return true;
}

function FunControllerID_onChange(event) {
    $('#FunActionName > option', _formElement).remove();
    $('#FunActionName', _formElement).combobox('SetSelected', '');

    if (event == undefined) {
        return false;
    }

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

function AddButton_onClick(srcElement) {
    $('#FunElmID').val('');
    $('#ExecAction').val(_ActionTypeAdd);
    $.blockUI({ message: '' });
    return true;
}

function RoleSetButton_onClick(srcElement) {
    var obj = JSON.parse($('input[name=para]', $(srcElement).closest('tr')).val());
    var para = '&FunElmID=' + obj.ElmID
        + '&SysID=' + obj.SysID
        + '&FunControllerID=' + obj.FunControllerID
        + '&FunActionName=' + obj.FunActionNM;

    _openWin('SystemFunElmRole', '/Sys/SystemFunElmRole', para, { width: 800, height: 600 });
    return false;
}

function UserSetButton_onClick(srcElement) {
    var obj = JSON.parse($('input[name=para]', $(srcElement).closest('tr')).val());
    var para = '&FunElmID=' + obj.ElmID
        + '&SysID=' + obj.SysID
        + '&FunControllerID=' + obj.FunControllerID
        + '&FunActionName=' + obj.FunActionNM;

    _openWin('SystemFunElmUser', '/Sys/SystemFunElmUser', para, { width: 800, height: 600 });
    return false;
}
