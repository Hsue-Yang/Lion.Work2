var _CloseJsErrMessage = "Close";
var _JsErrMessageBoxTitle = "Error";
var _JsErrMessageScriptOrOnEvent = "Can not enter the Script tag or tags where have On event";
var _Pager = { First: "First", Prev: "Prev", Next: "Next", Last: "Last" };
var _Symbol = { First: "<<", Prev: "<", Next: ">", Last: ">>" };
var _StrPager = { StrPager1: "Total ", StrPager2: " items. Each page with ", StrPager3: " items. Page ", StrPager4: " of ", StrPager5: "" };
var _PageSizeTitle = "one page have";
var _Currently = "Currently ";
var _Least = "Least ";

//#region 取得input 英文說明
function GetInputTypeResources(inputType) {
    var wResources = eval("({InputTypeResource:'',InputTypeCodeResource:'',Required:'Required',Select:'Required'})");
    switch (inputType) {
        case "TextBoxAlphanumeric":
            {
                wResources.InputTypeResource = "Alphanumeric ";
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxInteger":
            {
                wResources.InputTypeResource = "Number ";
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxDecimal":
            {
                wResources.InputTypeResource = "Number ";
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxNotChinese":
            {
                wResources.InputTypeResource = "Alphanumeric ";
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxChar8":
            {
                wResources.InputTypeResource = "Date Type ";
                wResources.InputTypeCodeResource = "YYYYMMDD";
                break;
            }
        case "TextBoxPassword":
            {
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxDatePicker":
            {
                wResources.InputTypeResource = "Date Type ";
                wResources.InputTypeCodeResource = "YYYYMMDD";
                break;
            }
        case "TextBox":
            {
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxNumber":
            {
                wResources.InputTypeResource = "Number ";
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxIdNo":
            {
                wResources.InputTypeResource = "Idno ";
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxInterval":
            {
                wResources.InputTypeResource = "Date Type ";
                wResources.InputTypeCodeResource = "YYYYMMDD";
                break;
            }
        case "TextBoxYearMonth":
            {
                wResources.InputTypeResource = "Date Type ";
                wResources.InputTypeCodeResource = "YYYYMM";
            }
    }

    return wResources
}

function GetBasicCodeResources(basicCodeType) {
    var BasicCodeResource = "";
    switch (basicCodeType) {
        case "Staff":
            {
                BasicCodeResource = "User ID 4 Code";
                break;
            }
        case "Agent":
            {
                BasicCodeResource = "Ageng Code 6 Code";
                break;
            }
        case "Airport":
            {
                BasicCodeResource = "Airport Code 3 Code";
                break;
            }
        case "City":
            {
                BasicCodeResource = "City Code 3 Code";
                break;
            }
        case "Country":
            {
                BasicCodeResource = "Country Code 2 Code";
                break;
            }
        case "Suppliers":
            {
                BasicCodeResource = "Suppliers 10 Code";
                break;
            }
        case "Currency":
            {
                BasicCodeResource = "Currency 3 Code";
                break;
            }
        case "Erat":
            {
                BasicCodeResource = "Exchange rate";
                break;
            }
        case "Hotel":
            {
                BasicCodeResource = "Hotel 8 Code";
                break;
            }
        case "Carr":
            {
                BasicCodeResource = "AirLine 2 Code";
                break;
            }

    }

    return BasicCodeResource;
}

//#endregion

//#region 設定小日曆英文格式
jQuery(function ($) {
    if ($("input[inputtype=TextBoxDatePicker]").length > 0) {
        $.datepicker.regional['en-US'] = {
            closeText: 'Close',
            prevText: '<Prev',
            nextText: 'Next>',
            currentText: 'Today',
            monthNames: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'friday', 'Sturday'],
            dayNamesMin: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'St'],
            dateFormat: 'yy-mm-dd', firstDay: 1,
            showMonthAfterYear: false,
            changeMonth: true,
            changeYear: true,
            isRTL: false
        };
        $.datepicker.setDefaults($.datepicker.regional['en-US']);
    }
});
//#endregion

