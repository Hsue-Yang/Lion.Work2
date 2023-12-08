var _formElement, _addConfirmDialog;

function SystemEDIFlowDetailForm_onLoad(formElement) {
    _formElement = formElement;
    _CreateAddConfirmDialog();
    $('#SysIDReadOnlyText').val($('#SysID').find('option:selected').text());
    $('#EDIFlowIDReadOnlyText').val($('#EDIFlowID').val());
    SCHFrequency_onChange();

    $('table.tblsearch #SCHFrequency', _formElement).combobox({
        width: 180,
        isRemoveButton: false
    });

    return true;
}

function Days_onClick(srcElement) {
    if (srcElement.checked) {
        var text;
        if (srcElement.value === '99') {
            text = JsMsg_MonthFinalDay;
        } else {
            text = srcElement.value;
        }
        
        var para = {
            id: 'SCHExecuteDay',
            text: text,
            isRevmove: true
        };

        var monthDate =
            $(document.createElement('input'))
                .attr({
                    'type': 'hidden',
                    'id': 'SCHExecuteDayList[0]',
                    'name': 'SCHExecuteDayList[0]',
                    value: srcElement.value
                });

        para.elementList = [monthDate];
        var tagStyle = Html.Tagstyle(para);
        $('#MonthlyDateBox').append(tagStyle);
        $('#MonthlyDateBox span.tagstyle').ToGroupElementReName();
    } else {
        var removeBtn = $('a[id^=SCHExecuteDay]', $('input[id^=SCHExecuteDayList][value=' + srcElement.value + ']').closest('span.tagstyle'))[0];
        SCHExecuteDayRemove_onClick(removeBtn);
    }
}

function SCHExecuteDayRemove_onClick(srcElement, keys) {
    $(srcElement).closest('span.tagstyle').remove();
    $('#MonthlyDateBox span.tagstyle').ToGroupElementReName();
}

function _CreateAddConfirmDialog() {
    _addConfirmDialog = $('#SetMonthlyDateDialog');
    _addConfirmDialog.dialog({
        title: JsMsg_SetMonthlyDate,
        autoOpen: false,
        modal: true,
        resizable: false,
        height: 280,
        width: 300,
        buttons: {
            click: {
                'class': 'btn',
                text: JsMsg_Confirm,
                click: function () {
                    _addConfirmDialog.dialog('close');
                    return false;
                }
            },
            reset: {
                'class': 'btn',
                text: JsMsg_Reset,
                click: function () {
                    $('input[name=Days]:checked').removeAttr('checked');
                    $('input[name=MonthFinalDay]:checked').removeAttr('checked');
                    $('#MonthlyDateBox').html('');
                    return false;
                }
            }
        }
    });
}

function SCHIntervalNum_onBlur(srcElement) {
    _ShowRemarkContent();
}

function SCHStartTime_onBlur(srcElement) {
    _ShowRemarkContent();
}

function SCHEndTime_onBlur(srcElement) {
    _ShowRemarkContent();
}

function SCHIntervalTime_onBlur(srcElement) {
    _ShowRemarkContent();
}

function ExecuteTime_onEnter(srcElement) {
    ExecuteTime_onBlur(srcElement);
}

function ExecuteTime_onBlur(srcElement) {
    if ($.trim(srcElement.value).length === 4) {
        if (srcElement.value.match('^(([0-1]?[0-9])|([2][0-3]))([0-5]?[0-9])?$')) {
            if ($('#ExecuteTimeListBox input[name^=ExecuteTimeList][value=' + srcElement.value + ']').length === 0) {
                var para = {
                    id: 'ExecuteTime',
                    text: srcElement.value,
                    isRevmove: true
                };
                var excuteTime = $(document.createElement('input')).attr({
                    'type': 'hidden',
                    'id': 'ExecuteTimeList[0]',
                    'name': 'ExecuteTimeList[0]',
                    value: srcElement.value
                });
                para.elementList = [excuteTime];
                var tagStyle = Html.Tagstyle(para);
                $('#ExecuteTimeListBox').append(tagStyle);
                srcElement.value = ''; //清除輸入框內容
                $('#ExecuteTimeListBox span.tagstyle').ToGroupElementReName();
            }
        } else {
            _AddJsErrMessage(JsMsg_ExecuteTimeFormateError); //請輸入正確24小時制時間格式(EX: 2035)
            _ShowJsErrMessageBox();
        }
    }
    _ShowRemarkContent();
}

function ExecuteTimeRemove_onClick(srcElement, keys) {
    $(srcElement).closest('span.tagstyle').remove();
    $('#ExecuteTimeListBox span.tagstyle').ToGroupElementReName();
    _ShowRemarkContent();
}

function _ShowRemarkContent() {
    var frequency = $('#SCHFrequency').val();
    var result = '';
    switch (frequency) {
        case 'Continuity':
            result = JsMsg_RemarkContentContinuity;
            break;
        case 'Daily':
        case 'Weekly':
        case 'Monthly':
            var executeTimeList = $('#ExecuteTimeListBox input[name^=ExecuteTimeList]');
            var executeTimeStr;
            var remarkContentFormat;
            if (executeTimeList.length > 0) {
                executeTimeStr = executeTimeList.map(function(idx, el) { return el.value; }).toArray().join('、');
            } else {
                executeTimeStr = '';
            }

            if (frequency === 'Daily') {
                remarkContentFormat = JsMsg_RemarkContentDaily;
            }
            else if (frequency === 'Weekly') {
                remarkContentFormat = JsMsg_RemarkContentWeekly;
            }
            else if (frequency === 'Monthly') {
                remarkContentFormat = JsMsg_RemarkContentMonthly;
            }

            result = remarkContentFormat.format($('#SCHIntervalNum').val(), executeTimeStr);
            break;
        case 'FixedTime':
            result = JsMsg_RemarkContentFixedTime.format($('#SCHStartTime').val(), $('#SCHEndTime').val(), $('#SCHIntervalTime').val());
            break;
    }
    $('#Remark').html(result);
}

function SCHFrequency_onChange(srcElement) {
    $('#TD_SCHIntervalTime').hide();
    $('#Label_SCHIntervalTime').hide();
    $('#TD_SCHEndTime').hide();
    $('#Label_SCHEndTime').hide();
    $('#FrequencyDetailBox').hide();
    $('#Label_SCHExecuteWeekly').hide();
    $('#TD_SCHExecuteWeekly').hide();
    $('#Label_SCHExecuteMonthly').hide();
    $('#TD_SCHExecuteMonthly').hide();
    $('#TD_SCHIntervalNum').removeAttr('colspan');

    switch ($('#SCHFrequency').val()) {
        case 'FixedTime':
            $('#TD_SCHIntervalTime').show();
            $('#Label_SCHIntervalTime').show();

            $('#TD_SCHEndTime').show();
            $('#Label_SCHEndTime').show();

            $('#TD_SCHDataDelay').removeAttr('colspan');
            break;
        case 'Daily':
            $('#FrequencyDetailBox').show();
            $('#TD_SCHDataDelay').attr('colspan', 5);
            $('#TD_SCHIntervalNum').attr('colspan', 3);
            break;
        case 'Weekly':
            $('#FrequencyDetailBox').show();
            $('#Label_SCHExecuteWeekly').show();
            $('#TD_SCHExecuteWeekly').show();
            $('#TD_SCHDataDelay').attr('colspan', 5);
            break;
        case 'Monthly':
            $('#FrequencyDetailBox').show();
            $('#Label_SCHExecuteMonthly').show();
            $('#TD_SCHExecuteMonthly').show();
            $('#TD_SCHDataDelay').attr('colspan', 5);
            break;
        default:
            $('#TD_SCHDataDelay').attr('colspan', 5);
    }
    _ShowRemarkContent();
}

//----Button----//

function CalendarBtn_onClick(srcElement) {
    _addConfirmDialog.dialog('open');
    return false;
}

function AddButton_onClick(srcElement) {
    var result = _FormValidation();
    if (ValidationSCHFrequency(srcElement) === false) {
        return false;
    }
    $('#ExecAction').val(_ActionTypeAdd);
    if ($('#SCHStartTime').val() > 235959999) { $('#SCHStartTime').val(235959999) }
    if ($('#SCHEndTime').val() > 235959999) { $('#SCHEndTime').val(235959999) }
    if ($('#MonthFinalDay:checked').val()) {  }
    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function UpdateButton_onClick(srcElement) {
    var result = _FormValidation();
    if (ValidationSCHFrequency(srcElement) === false) {
        return false;
    }
    $('#ExecAction').val(_ActionTypeUpdate);
    if ($('#SCHStartTime').val() > 235959999) { $('#SCHStartTime').val(235959999) }
    if ($('#SCHEndTime').val() > 235959999) { $('#SCHEndTime').val(235959999) }
    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function ValidationSCHFrequency(parameters) {
    var result = true;
    var keepLogDayResult = true;
    var keepLogDay = $('input[id=KeepLogDay]').val();

    if (keepLogDay.match(/[0-9]*/)[0] !== keepLogDay) {
        _AddJsErrMessage(JsMsg_IsValidKeepLogDayNumRequired_Failure);
        keepLogDayResult = false;
    }

    var vailidationSchIntervalNum = function () {
        if ($('#SCHIntervalNum').val() === '') {
            _AddJsErrMessage(JsMsg_IsValidSCHIntervalNumRequired_Failure);
            return false;
        }
        return true;
    };

    var vailidationExecuteTimeListBox = function () {
        if ($('#ExecuteTimeListBox span').size() === 0) {
            _AddJsErrMessage(JsMsg_IsValidExecuteTimeRequired_Failure);
            return false;
        }
        return true;
    };

    switch ($('#SCHFrequency').val()) {
        case 'Daily':
            result = vailidationSchIntervalNum();
            result = vailidationExecuteTimeListBox() && result;
            break;
        case 'Weekly':
            result = vailidationSchIntervalNum();
            result = vailidationExecuteTimeListBox() && result;

            if ($('#SCHExecuteWeeklyList:checked').size() === 0) {
                _AddJsErrMessage(JsMsg_IsValidSCHExecuteWeeklyListRequired_Failure);
                result = false;
            }
            break;
        case 'Monthly':
            result = vailidationSchIntervalNum();
            result = vailidationExecuteTimeListBox() && result;

            if ($('#MonthlyDateBox span').size() === 0) {
                _AddJsErrMessage(JsMsg_IsValidSCHExecuteDayListRequired_Failure);
                result = false;
            }
            break;
        case 'FixedTime':
            if ($('#SCHIntervalTime').val() === '') {
                _AddJsErrMessage(JsMsg_IsValidSCHIntervalTimeRequired_Failure);
                result = false;
            }
            if ($('#SCHEndTime').val() === '') {
                _AddJsErrMessage(JsMsg_IsValidSCHEndTimeRequired_Failure);
                result = false;
            }
            break;
    }

    if (result === false || keepLogDayResult === false) {
        _ShowJsErrMessageBox();
    }

    return (result === true && keepLogDayResult === true) === true;
}

function DeleteButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    _alert('dialog_Confirm');
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}

function ConfirmOKButton_onClick(srcElement) {
    var result = _FormValidation();

    $('#ExecAction').val(_ActionTypeDelete);
    if (result) {
        _formElement.submit();
    }

    _btnUnblockUI(this, false);
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}