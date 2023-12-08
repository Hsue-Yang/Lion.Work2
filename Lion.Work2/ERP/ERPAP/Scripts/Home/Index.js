var _formElement;

function IndexForm_onLoad(formElement) {
    _formElement = formElement;

    if ($('#UserID').val() !== '') {
        $('#UserPassword').focus();
    } else {
        $('#UserID').focus();
    }

    if ($(window).height() <= 600) {
        $(_formElement).height(600);
    } else {
        $(_formElement).height($(window).height() - 20);
    }

    LoginType_onChange($('#LoginType')[0]);
    return true;
}

$(function() {
    $('#' + _formElement.id).keypress(function(e) {
        if ((e.which && e.which === 13) || (e.keyCode && e.keyCode === 13)) {
            var result = _FormValidation();
            $('#ExecAction').val(_ActionTypeUpdate);
            if (result) {
                $.blockUI({ message: '' });
                _formElement.submit();
            }
        }
    });

    $('#UserID').blur(function(e) {
        if (this.value === '' || this.value == undefined) {
            _ClossJsErrMessageBox();
        }
    });

    $('#UserPassword').blur(function(e) {
        if (this.value === '' || this.value == undefined) {
            _ClossJsErrMessageBox();
        }
    });

    $('#UserPassword').keypress(function(e) {
        if ((e.which >= 65 && e.which <= 90) || (e.which >= 97 && e.which <= 122)) {
            //按下Shift鍵時，卻輸入小寫字母
            //沒按下Shift鍵，卻輸入大寫字母
            if (((e.which >= 65 && e.which <= 90) && !e.shiftKey) ||
            ((e.which >= 97 && e.which <= 122) && e.shiftKey)) {
                $('#CapsLockRemind').show();
            } else {
                $('#CapsLockRemind').hide();
            }
        }
    });

    /*
    var qrCode = $.connection.qrCode;
    
    $.connection.hub.start().done(function () {
        qrCode.server.generator();
    });

    qrCode.client.Login = function (pincode) {
        $('#PingCode').val(pincode);
        $(_formElement).attr('action', '/Home/QRCodeLogin');
        $('#ExecAction').val(_ActionTypeUpdate);
        _formElement.submit();
    };

    qrCode.client.Generator = function (pincode) {
        $('#QRCodeLogin td').html('<img width=120 src="data:image/png;base64,' + pincode + '" />');
        setTimeout(qrCode.server.generator, 59000);
    };
    */
});

function LoginType_onChange(srcElement) {
    //$('#QRCodeLogin').hide();
    $('#AccountPassWord').hide();
    $('#LoginOption').hide();

    if (srcElement.value === 'AccountPSW') {
        $('#AccountPassWord').show();
        $('#LoginOption').show();
        $('#UserPassword').prop('type', 'password').prop('inputtype', 'TextBoxPassword').val('');
        $('#UserPassword').attr('maxlength', '10').attr('title', '通行碼：10碼 必須輸入');
        //$('#OTPRemind').hide();
        return false;
    } else if (srcElement.value === 'DigipassOTP') {
        $('#AccountPassWord').show();
        $('#LoginOption').hide();
        $('#UserPassword').prop('type', 'text').prop('inputtype', 'TextBoxNumber').val('');
        $('#UserPassword').attr('maxlength', '14').attr('title', '通行碼：7碼 必須輸入');
        //$('#OTPRemind').show();
        return false;
    }

    //$('#QRCodeLogin').show();

    return true;
}

function LoginButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (result) {
        $.blockUI({ message: '' });
        $(_formElement).attr('action', '/Home/Index');
        _formElement.submit();
    }
}

function CultureID_onChange(srcElement) {
    $('#ExecAction').val(_ActionTypeSelect);
    $.blockUI({ message: '' });
    _formElement.submit();
}

function CultureLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });

    $('#CultureID').val(keys[1]);

    $('#ExecAction').val(_ActionTypeSelect);
    return true;
}