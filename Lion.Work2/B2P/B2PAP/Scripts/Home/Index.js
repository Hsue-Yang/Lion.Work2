var _formElement;

function IndexForm_onLoad(formElement) {
    _formElement = formElement;

    $('#LoginOption input').attr('disabled', true);
    $('#LoginOption').css('color', 'Silver');

    if($('#UserID').val() != "") {
        $('#UserPassword').focus();
    } else {
        $('#UserID').focus();
    }

    if ($(window).height() <= 600) {
        $(_formElement).height(600);
    }
    else {
        $(_formElement).height($(window).height() - 20);
    }

    return true;
}

$(function () {
    $('#' + _formElement.id).keypress(function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            var Result = _FormValidation();
            $('#ExecAction').val(_ActionTypeUpdate);
            if (Result) {
                $.blockUI({ message: '' });
                _formElement.submit();
            }
        }
    });

    $('#UserID').blur(function (e) {
        if (this.value == '' || this.value == undefined) {
            _ClossJsErrMessageBox();
        }
    });

    $('#UserPassword').blur(function (e) {
        if (this.value == '' || this.value == undefined) {
            _ClossJsErrMessageBox();
        }
    });

    $('#UserPassword').keypress(function (e) {
        if ((e.which >= 65 && e.which <= 90) || (e.which >= 97 && e.which <= 122)) {
            //按下Shift鍵時，卻輸入小寫字母
            //沒按下Shift鍵，卻輸入大寫字母
            if (((e.which >= 65 && e.which <= 90) && !e.shiftKey) ||
                ((e.which >= 97 && e.which <= 122) && e.shiftKey)) {
                $('#CapsLockRemind').show();
            }
            else {
                $('#CapsLockRemind').hide();
            }
        }
    });
});

function LoginButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
	$('#UserID').val($.trim($('#UserID').val()));
    if (Result) {
        $.blockUI({ message: '' });
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