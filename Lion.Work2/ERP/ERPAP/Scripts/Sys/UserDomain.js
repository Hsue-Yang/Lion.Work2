var _formElement;

function UserDomainForm_onLoad(formElement) {
    _formElement = formElement;

    $('table.tblsearch #DomainPath', _formElement).combobox({
        isRemoveButton: false
    });

    return true;
}

function TRLinkFunKey_onClick(srcElement, keys) {
    var domainGroupObject = $('tr[id="DomainGroupList[' + keys[1] + ']"]');
    domainGroupObject.toggle();

    if (domainGroupObject.is(':hidden') === false) {
        domainGroupObject.html('');
        $.ajax({
            url: '/Sys/GetUserDoaminInfoList',
            type: 'POST',
            data: {
                UserEMailAccount: keys[2],
                DomainPath: $('#DomainPath').val()
            },
            dataType: 'json',
            async: false,
            success: function(result) {
                if (result) {
                    var domainGroup = '';
                    var titleDomainAccount = $('<th>').html(JsMsg_DomainAccount);
                    var titleDomainNm = $('<th>').html(JsMsg_DomainName);
                    var titleDomainGroup = $('<th>').html(JsMsg_DomainGroupID);
                    var dataDomainAccount = $('<td>').html(result.DomainAccount);
                    var dataDomainNm = $('<td>').html(result.DomainNM);

                    var table = $(document.createElement('table'));
                    var header = $(document.createElement('tr')).append(titleDomainAccount, titleDomainNm, titleDomainGroup);
                    var data = $(document.createElement('tr')).append(dataDomainAccount, dataDomainNm);
                    var td = $(document.createElement('td')).attr('colspan', '6').append(table);

                    $(result.DomainGroup).each(function(idx, el) {
                        domainGroup += el + '</br>';
                    });

                    data.append($(document.createElement('td')).html(domainGroup));
                    table.append(header).append(data).addClass('tblzebra');
                    domainGroupObject.append($(document.createElement('td')).after(td));
                }
            },
            error: function() {
                _AddJsErrMessage(JsMsg_GetSystemAPIGroupList);
                _ShowJsErrMessageBox();
            }
        });
    }

    return false;
}

function SelectButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function Help03Button_onClick(srcElement) {
    var vMapFields = new Array(3);
    vMapFields[1] = 'UserID';
    vMapFields[2] = 'UserNM';
    _hISearch(vMapFields, 'newwindow', _enumPUBAP + '/Help/Ishlp03', 'Name=' + encodeURI($.trim($('#UserID').val())));
}

function DomainPath_onChange(srcElement) {
    if (srcElement.value.indexOf('liontech') > -1) {
        $.blockUI({ message: $('#dialog_Confirm') });
    }
    return true;
}

function OKButton_onClick(srcElement) {
    if ($('#dialog_Confirm input[name=PWD]').val() !== '') {
        $.ajax({
            url: '/Sys/GetDomainLoginResult',
            type: 'POST',
            data: {
                pwd: $('#dialog_Confirm input[name=PWD]').val()
            },
            dataType: 'json',
            async: true,
            success: function (result) {
                if (result.isLogin === false) {
                    $('#DomainPath').val($('#DomainPath option:eq(0)').val());
                    _AddJsErrMessage(JsMsg_Login_Failure);
                    _ShowJsErrMessageBox();
                }
                _btnUnblockUI($('#dialog_Confirm'));
            },
            error: function () {
                _AddJsErrMessage(JsMsg_Login_Failure);
                _ShowJsErrMessageBox();
                $('#' + _JsErrMessageBox).css('z-index', 9999);
            }
        });
    } else {
        _AddJsErrMessage(JsMsg_LoginPWD_Required);
        _ShowJsErrMessageBox();
        $('#' + _JsErrMessageBox).css('z-index', 9999);
        return false;
    }

    return true;
}

function CancelButton_onClick(srcElement) {
    $('#DomainPath').val($('#DomainPath option:eq(0)').val());
    _btnUnblockUI($('#dialog_Confirm'));
}