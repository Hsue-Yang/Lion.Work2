var _CloseJsErrMessage = "關閉";
var _JsErrMessageBoxTitle = "錯誤";
var _JsErrMessageScriptOrOnEvent = "不可輸入Script標籤或是標籤內有On事件";
var _Pager = { First: "首頁", Prev: "上頁", Next: "下頁", Last: "末頁" };
var _Symbol = { First: "<<", Prev: "<", Next: ">", Last: ">>" };
var _StrPager = { StrPager1: "共", StrPager2: "筆，每頁", StrPager3: "筆，目前第", StrPager4: "頁，共", StrPager5: "頁" };
var _PageSizeTitle = "每頁幾筆";
var _Currently = "目前";
var _Least = "至少";

//#region 取得input 中文說明
function GetInputTypeResources(inputType) {
    var wResources = eval("({InputTypeResource:'',InputTypeCodeResource:'',Required:'必須輸入',Select:'必須選取'})");
    switch (inputType) {
        case "TextBoxAlphanumeric":
            {
                wResources.InputTypeResource = "英數字";
                wResources.InputTypeCodeResource = "碼";
                break;
            }
        case "TextBoxInteger":
            {
            	wResources.InputTypeResource = "數字";
            	wResources.InputTypeCodeResource = "碼";
                break;
            }
        case "TextBoxDecimal":
            {
            	wResources.InputTypeResource = "數字";
            	wResources.InputTypeCodeResource = "碼";
                break;
            }
        case "TextBoxNotChinese":
            {
                wResources.InputTypeResource = "英數字";
                wResources.InputTypeCodeResource = "碼";
                break; 
            }
        case "TextBoxChar8":
            {
                wResources.InputTypeResource = "日期格式";
                wResources.InputTypeCodeResource = "YYYYMMDD";
                break;
            }
        case "TextBoxPassword":
            {
                wResources.InputTypeCodeResource = "碼";
                break;
         }
        case "TextBoxDatePicker":
            {
                wResources.InputTypeResource = "日期格式";
                wResources.InputTypeCodeResource = "YYYYMMDD";
                break;
            }
        case "TextBox":
            {
                wResources.InputTypeCodeResource = "字";
                break;
            }
        case "TextBoxNumber": 
            {
            	wResources.InputTypeResource = "數字";
            	wResources.InputTypeCodeResource = "碼";
                break;
            }
        case "TextBoxIdNo": 
            {
                wResources.InputTypeResource = "身份證字號";
                wResources.InputTypeCodeResource = "碼";
                break;
            }
        case "TextBoxInterval": 
            {
                wResources.InputTypeResource = "日期格式";
                wResources.InputTypeCodeResource = "YYYYMMDD";
                break;
			}
        case "TextBoxYearMonth": 
			{
				wResources.InputTypeResource = "日期格式";
				wResources.InputTypeCodeResource = "YYYYMM";
			}
    }

    return wResources
}

function GetBasicCodeResources(basicCodeType) {
    var BasicCodeResource= "";
    switch (basicCodeType) {
        case "Staff":
            {
                BasicCodeResource = "使用者代碼4碼";
                break;
            }
        case "Agent":
            {
                BasicCodeResource = "同行公司代碼6碼";
                break;
            }
        case "Airport":
            {
                BasicCodeResource = "機場代碼3碼";
                break;
            }
        case "City":
            {
                BasicCodeResource = "城市代碼3碼";
                break;
            }
        case "Country":
            {
                BasicCodeResource = "國家代碼2碼";
                break;
            }
        case "Suppliers": 
            {
                BasicCodeResource = "供應商代碼10碼";
                break;
            }
        case "Currency":
            {
                BasicCodeResource = "幣別代碼3碼";
                break;
            }
        case "Erat":
            {
                BasicCodeResource = "匯率";
                break;
            }
        case "Hotel":
            {
                BasicCodeResource = "旅館代碼8碼";
                break;
            }
        case "Carr":
           	{
           		BasicCodeResource = "航空公司代碼2碼";
           		break;
			}
    }
    return BasicCodeResource;
}

//#endregion

//#region 設定日曆中文格式
jQuery(function ($) {
    if ($("input[inputtype=TextBoxDatePicker]").length > 0) {
        $.datepicker.regional['zh-TW'] = {
            closeText: '關閉',
            prevText: '<上月',
            nextText: '下月>',
            currentText: '今天',
            monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
            monthNamesShort: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
            dayNames: ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
            dayNamesShort: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],
            dayNamesMin: ['日', '一', '二', '三', '四', '五', '六'],
            dateFormat: 'yy-mm-dd', firstDay: 1,
            showMonthAfterYear: false,
            changeMonth: true,
            changeYear: true,
            isRTL: false
        };
        $.datepicker.setDefaults($.datepicker.regional['zh-TW']);
    }
});
//#endregion

