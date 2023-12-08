/// <reference path="/Scripts/jquery-1.5.1-vsdoc.js" />
var _ActionTypeAdd = "Add";
var _ActionTypeCopy = "Copy";
var _ActionTypeUpdate = "Update";
var _ActionTypeQuery = "Query";
var _ActionTypeDelete = "Delete";
var _ActionTypeSelect = "Select";
var _JsErrMessageBox = "JsErrMessageBox";
var _JsErrMessageBoxLabel = "JsErrMessageBoxLabel";
var _JsOtherErrMessageBoxLabel = "JsOtherErrMessageBoxLabel";
var _IsRequired = "isrequired";
var _Err = "Err";
var _display = "display";
var _decData = new QSData(); //代碼help用
var _isScriptOrOnEventInput = false;
var _workflowAPIAction;
var _workflowAPIResult;

//#region menu_onLoad
function _UserMenu_onLoad() {
    $('div[id="MenuText"]').each(function () {
        var _this = $(this),
            _thisImg = _this.find('#MenuImg'),
            _thisItems = _this.find('#MenuItems');

        _this.hover(function () {
            _this.addClass("Selected");
            _thisImg.removeClass("ImgExpand").addClass("ImgCollapse");
            _thisItems.css('margin-top', this.offsetHeight - 18).stop(true, true).fadeIn(200);
        }, function () {
            _this.removeClass("Selected");
            _thisImg.removeClass("ImgCollapse").addClass("ImgExpand");
            _thisItems.stop(true, true).fadeOut(200);
        });
    });
};
//#endregion

//#region Form 共用
function _FormSubmit(formID) {
    var Result;
    _setTitle(); //設定input的title
    _SetErrMessageBox(); //設定錯誤訊息視窗

    eval("if(typeof(" + formID + "_onLoad)==='function') { Result = " + formID + "_onLoad(document.getElementById('" + formID + "')); }");
    if (Result) {
        //預留後續處理位置
    }
    formID = '#' + formID;
    $(formID).submit(function () { //擋submit button 讓pg
        return false;
    });
}
//#endregion

//#region TextBox共用
function _TextBox_onBlur(srcElement) {
    var Result = true;
    var inputType = $(srcElement).attr('inputtype');
    var ID = $(srcElement).attr('id');
    var ElementId = "";
    var value = $(srcElement).val();
    var IsValidation = $(srcElement).attr("validation");
    var labelId = "#" + _Err + ID.replace(/[\[]/g, "\\[").replace(/[\]]/g, "\\]").replace(/[\.]/g, "\\.");
    var tObj = $(labelId);

    //tObj.hide().attr(_display, _display);
    //tObj.remove();
    tObj.hide();
    value = value.replace(/(^\t*)|(\t*$)/g, '');
    value = value.replace(/(^\r*)|(\r*$)/g, '');
    value = value.replace(/(^\n*)|(\n*$)/g, '');
    value = value.replace(/(^\v*)|(\v*$)/g, '');

    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (IsValidation == "") {
        $(srcElement).val(value);
        Result = _Validation(inputType, value, srcElement, tObj);
    }

    if (Result) {
        eval("if(typeof(" + ElementId + "_onBlur)==='function'){ Result = " + ElementId + "_onBlur(srcElement); }");
        tObj.remove();
    } else {
        //tObj.show().attr(_display, "");
        if (tObj.html() == undefined) {
            if (_isScriptOrOnEventInput) {
                _AddJsErrMessage(_JsErrMessageScriptOrOnEvent, ID);
            } else {
                _AddJsErrMessage(_getTextBoxTitle($(srcElement)), ID);
            }
        } else {
            tObj.show();
        }
    }

    if (value != srcElement.getAttribute("originalvalue")) {
        $(srcElement).addClass("Changed");
    } else {
        $(srcElement).removeClass("Changed");
    }

    _ShowJsErrMessageBox();

    return Result;
}

function _TextBox_onKeyPress(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";

    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (Result) {
        eval("if(typeof(" + ElementId + "_onKeyPress)==='function'){ Result = " + ElementId + "_onKeyPress(srcElement); }");

        if (Result && event.keyCode == 13) {
            Result = _TextBox_onEnter(srcElement);
        }
    }

    return Result;
}

function _TextBox_onEnter(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";

    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (Result) {
        eval("if(typeof(" + ElementId + "_onEnter)==='function'){ Result = " + ElementId + "_onEnter(srcElement); }");
    }

    return Result;
}
//#endregion

//#region AutoCompleteTextBox 共用
function _AutoCompleteTextBox_onChange(hiddenID, srcElement) {
    var inputHidden = $(srcElement).nextAll('input[id="' + srcElement.id.replace('_text', '') + '"]');
    var inputValue = srcElement.value.toUpperCase();

    //判斷是否有" | "
    if (inputValue.indexOf(" | ") !== -1) {
        inputValue = inputValue.substring(inputValue.indexOf("|") + 1, inputValue.length).trim();
    }

    var form = _GetParentElementByTag(srcElement, "FORM");
    if (form != null) {

        if (inputValue.indexOf(" | ") !== -1) {
            $(srcElement).val('');
            inputHidden.val('');
        }

        if ($(srcElement).val() === '') {
            $(srcElement).val(inputValue);
            inputHidden.val(inputValue);
        }

        $('#' + srcElement.id.replace('_text', '') + '_autoCompleteState', form).val('2');
    } else {

        $(srcElement).val('');
        inputHidden.val('');

        if ($(srcElement).val() === '') {
            $(srcElement).val(inputValue);
            inputHidden.val(inputValue);
        }

        $('#' + srcElement.id.replace('_text', '') + '_autoCompleteState').val('2');
    }
}

function _AutoCompleteTextBox_GetList(hiddenID, formElement, controllerID, actionName, clearText) {
    if (clearText == null || clearText == undefined) {
        clearText = false;
    }

    var result, condition;
    var inputObj = $('#' + hiddenID + '_text');
    var inputValue;
    var isEnNumber = true;
    var elementID = '';

    $('#' + hiddenID + '_text', $(formElement)).autocomplete({
        source: function (request, response) {
            
            if (inputObj.prop('id').split('__').length > 1) {
                elementID = inputObj.prop('id').split('__')[1];
            } else {
                elementID = inputObj.prop('id');
            }

            eval("if(typeof(" + elementID + "_beforePost)=='function') { result = " + elementID + "_beforePost(inputObj[0]); }");
            if (result != '' && result != undefined && typeof result == "string") {
                condition = result;
            } else {
                condition = request.term;
            }
            
            //判斷是否有" | "
            inputValue = $('#' + hiddenID + '_text').val();
            if (inputValue.indexOf(" | ")!=-1) {
                inputValue = inputValue.substring(inputValue.indexOf("|")+1, inputValue.length).trim();
                condition = inputValue;
                $('#' + hiddenID, $(formElement)).val(inputValue);    
            }
            
            $.post('/' + controllerID + '/' + actionName, { condition: condition }, function (data) {
                //(inputValue.charCodeAt(i) > 19968 && inputValue.charCodeAt(i) < 40869) 中文
                //48~57 數字 65~90 EN大寫 97~122 EN小寫
                isEnNumber = true; //判斷是否只輸入英數
                if (inputValue.length != 0) {
                    for (i = 0; i < inputValue.length; i++) {
                        if (inputValue.charCodeAt(i) > 122)
                            isEnNumber = false;
                    }
                }

                if (data.length == 0) {
                    response(null);
                }
                if (data.length == 1 && isEnNumber == true) {
                    $('#' + hiddenID + '_text', $(formElement)).val(data[0].Text + ' | ' + data[0].Value);
                    $('#' + hiddenID, $(formElement)).val(data[0].Value);
                    $('#' + hiddenID + '_autoCompleteState', $(formElement)).val('1'); //autoComplete狀態 1為選定
                    
                    var srcElement = $('#' + hiddenID + '_text', $(formElement))[0];
                    eval("if(typeof(" + hiddenID + "_text_onSelected)=='function') { handleing = " + hiddenID + "_text_onSelected(srcElement); }");
                    $('#' + hiddenID, $(formElement)).focus();

                    response(null);
                }
                else if (data.length >= 1 && (data[0].Text != 'undefined')) {
                    response($.map(data, function (item) {
                        return {
                            label: (item.Text + ' | ' + item.Value),
                            value: (item.Value)
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            if (ui.item) {
                $('#' + hiddenID + '_text', $(formElement)).val(ui.item.label);
                $('#' + hiddenID, $(formElement)).val(ui.item.value);
                $('#' + hiddenID + '_autoCompleteState', $(formElement)).val('1'); //autoComplete狀態 1為選定
                ui.item.value = ui.item.label;
                
                var srcElement = $('#' + hiddenID + '_text', $(formElement))[0];
                eval("if(typeof(" + hiddenID + "_text_onSelected)=='function') { handleing = " + hiddenID + "_text_onSelected(srcElement); }");
            }
        },
        focus: function (event, ui) {
            return false;
        }
    });
    if ($('#' + hiddenID + '_text').val() == '') {
        $('#' + hiddenID).val(inputValue);
        $('#' + hiddenID + '_text').val(inputValue);
    }

    //直接帶出 ID & Name
    if (clearText == false) {
        var textValue = $('#' + hiddenID).val();
        _AutoComplete_getName();
    }
    if (clearText == true) {
        $('#' + hiddenID).val('');
        $('#' + hiddenID + '_text', $(formElement)).val('');
    }

    //按下Enter後帶出唯一筆
    $(function () {
        $('#' + _formElement.id).keypress(function (e) {
            if (isEnNumber == false && ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13))) {
                _AutoComplete_getName();
            }
        });
    });

    function _AutoComplete_getName() {
        inputValue = $('#' + hiddenID + '_text').val();
        $.post('/' + controllerID + '/' + actionName + '?condition=' + encodeURI(inputValue), function (data) {
            if (data.length == 1 && inputValue != '') {
                $('#' + hiddenID + '_text', $(formElement)).val(data[0].Text + ' | ' + data[0].Value);
                $('#' + hiddenID, $(formElement)).val(data[0].Value);
                $('#' + hiddenID + '_autoCompleteState', $(formElement)).val('1'); //autoComplete狀態 1為選定

                var srcElement = $('#' + hiddenID + '_text', $(formElement))[0];
                eval("if(typeof(" + hiddenID + "_text_onSelected)=='function') { handleing = " + hiddenID + "_text_onSelected(srcElement); }");
                $('#' + hiddenID, $(formElement)).focus();
                $(".ui-menu-item").parent().hide();
            } else if (data.length > 1) {
                $('#' + hiddenID).val(inputValue.toUpperCase());
                $('#' + hiddenID + '_text').val(inputValue.toUpperCase());
            }
        });
    }
}
//#endregion

//#region TextArea 共用
function _TextArea_onBlur(srcElement) {
    var Result = true;
    var inputType = $(srcElement).attr('inputtype');
    var ID = $(srcElement).attr('id');
    var value = $(srcElement).val();
    var IsValidation = $(srcElement).attr("validation");
    var ElementId = "";
    var labelId = "#" + _Err + ID.replace(/[\[]/g, "\\[").replace(/[\]]/g, "\\]").replace(/[\.]/g, "\\.");
    var tObj = $(labelId);

    //tObj.hide().attr(_display, _display);
    tObj.hide();

    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (IsValidation == "") {
        $(srcElement).val(value);
        Result = _Validation(inputType, value, srcElement, tObj);
    }

    if (Result) {
        eval("if(typeof(" + ElementId + "_onBlur)==='function'){ Result = " + ElementId + "_onBlur(srcElement); }");
        tObj.remove();
    } else {
        //tObj.show().attr(_display, "");
        if (tObj.html() == undefined) {
            if (_isScriptOrOnEventInput) {
                _AddJsErrMessage(_JsErrMessageScriptOrOnEvent, ID);
            } else {
                _AddJsErrMessage(_getTextBoxTitle($(srcElement)), ID);
            }
        } else {
            tObj.show();
        }
    }

    if (value != srcElement.getAttribute("originalvalue")) {
        $(srcElement).addClass("Changed");
    } else {
        $(srcElement).removeClass("Changed");
    }

    _ShowJsErrMessageBox()

    return Result;
}

function _TextArea_onKeyPress(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";

    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (Result) {
        eval("if(typeof(" + ElementId + "_onKeyPress)==='function'){ Result = " + ElementId + "_onKeyPress(srcElement); }");

        if (Result && event.keyCode == 13) {
            Result = _TextArea_onEnter(srcElement);
        }
    }

    return Result;
}

function _TextArea_onEnter(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";

    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (Result) {
        eval("if(typeof(" + ElementId + "_onEnter)==='function'){ Result = " + ElementId + "_onEnter(srcElement); }");
    }

    return Result;
}
//#endregion

//#region Radio共用
function _InputRadioButton_onClick(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";
    
    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (Result) {
        eval("if(typeof(" + ElementId + "_onClick)==='function'){ Result = " + ElementId + "_onClick(srcElement); }");
    }

    if ((srcElement.checked == false && srcElement.getAttribute("originalvalue") == "checked") ||
        (srcElement.checked == true && srcElement.getAttribute("originalvalue") == "")) {
        $(srcElement).addClass("Changed");
    } else {
        $(srcElement).removeClass("Changed");
    }
    
    $('input:radio').each(function(){
        if(!$(this).attr("checked")) {
            $(this).parent().removeClass("Changed");
        }
    });
    
    if ((srcElement.checked == false && srcElement.getAttribute("originalvalue") == "checked") ||
        (srcElement.checked == true && srcElement.getAttribute("originalvalue") == "")) {
        $(srcElement).parent().addClass("Changed");
    }
    

    return Result;
}

function _InputRadioButton_onKeyPress(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";

    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (Result) {
        eval("if(typeof(" + ElementId + "_onKeyPress)==='function'){ Result = " + ElementId + "_onKeyPress(srcElement); }");

        if (Result && event.keyCode == 13) {
            Result = _InputRadioButton_onEnter(srcElement);
        }
    }

    return Result;
}

function _InputRadioButton_onEnter(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";

    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (Result) {
        eval("if(typeof(" + ElementId + "_onEnter)==='function'){ Result = " + ElementId + "_onEnter(srcElement); }");
    }

    return Result;
}
//#endregion  

//#region CheckBox共用
function _InputCheckBox_onClick(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";
    
    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (Result) {
        eval("if(typeof(" + ElementId + "_onClick)==='function'){ Result = " + ElementId + "_onClick(srcElement); }");
    }

    if ((srcElement.checked == false && srcElement.getAttribute("originalvalue") == "checked") ||
        (srcElement.checked == true && srcElement.getAttribute("originalvalue") == "")) {
        $(srcElement).addClass("Changed");
    } else {
        $(srcElement).removeClass("Changed");
    }
    
    if ((srcElement.checked == false && srcElement.getAttribute("originalvalue") == "checked") ||
        (srcElement.checked == true && srcElement.getAttribute("originalvalue") == "")) {
        $(srcElement).parent().addClass("Changed");
    } else {
        $(srcElement).parent().removeClass("Changed");
    }
    
    return Result;
}

function _InputCheckBox_onKeyPress(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";

    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (Result) {
        eval("if(typeof(" + ElementId + "_onKeyPress)==='function'){ Result = " + ElementId + "_onKeyPress(srcElement); }");

        if (Result && event.keyCode == 13) {
            Result = _InputCheckBox_onEnter(srcElement);
        }
    }

    return Result;
}

function _InputCheckBox_onEnter(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";

    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (Result) {
        eval("if(typeof(" + ElementId + "_onEnter)==='function'){ Result = " + ElementId + "_onEnter(srcElement); }");
    }

    return Result;
}
//#endregion 

//#region InputComboBox共用
function _InputComboBox_onChange(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";
    
    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (Result) {
        eval("if(typeof(" + ElementId + "_onChange)==='function'){ Result = " + ElementId + "_onChange(srcElement); }");
    }

    if (srcElement.value != srcElement.getAttribute("originalvalue")) {
        $(srcElement).addClass("Changed");
    } else {
        $(srcElement).removeClass("Changed");
    }
    
    return Result;
}
//#endregion 

//#region _ButtonClient_onClick共用
function _ButtonClient_onClick(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";
    var rowName = "";
    
    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (ID.split(".").length > 1) {
        rowName = ID.split(".")[0];
    }

    _ClossJsErrMessageBox();

    if (Result) {
        eval("if(typeof(" + ElementId + "_onClick)==='function'){ Result = " + ElementId + "_onClick(srcElement,'" + rowName + "'); }");
    }
    return Result;
}
//#endregion

//#region _ButtonSubmit_onClick 共用
function _ButtonSubmit_onClick(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";
    $(srcElement).attr("disabled", "true");
    
    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    _ClossJsErrMessageBox();

    if (Result) {
        eval("if(typeof(" + ElementId + "_onClick)==='function'){ Result = " + ElementId + "_onClick(srcElement); }");
    }
    if (Result) {
       _submit($(srcElement));
    };
    $(srcElement).removeAttr("disabled");
    return Result;
}
//#endregion

//#region _TextsSubmitLink_onClick 共用
function _TextsSubmitLink_onClick(srcElement, key) {
    srcElement = srcElement[0];
    var keys = key.split(";");
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";
    
    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    _ClossJsErrMessageBox();

    //if ($("select[id=PageIndex]").length > 0) {
    //    $("select[id=PageIndex]").val("1");
    //}

    if (Result) {
        eval("if(typeof(" + ElementId + "_onClick)==='function'){ Result = " + ElementId + "_onClick(srcElement,keys); }");
    }
    if (Result) {
        _submit($(srcElement));
    };
}
//#endregion

//#region _TextsClientLink_onClick 共用
function _TextsClientLink_onClick(srcElement, key) {
    srcElement = srcElement[0];
    var keys = key.split(";");
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";
    
    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    _ClossJsErrMessageBox();

    //if ($("select[id=PageIndex]").length > 0) {
    //    $("select[id=PageIndex]").val("1");
    //}

    if (Result) {
        eval("if(typeof(" + ElementId + "_onClick)==='function'){ Result = " + ElementId + "_onClick(srcElement,keys); }");
    }
    return Result;
}
//#endregion

//#region _ImageButton_onClick共用
function _ImageButton_onClick(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";
    
    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (Result) {
        eval("if(typeof(" + ElementId + "_onClick)==='function'){ Result = " + ElementId + "_onClick(srcElement); }");
    }
    return Result;
}
//#endregion 

//#region Pager 共用
function _PagerReady() {    
    var $div = $("div[symbol]");
    
    if($div.length > 0) {
        $div.filter("div[id=Pager_First]").each(function () { this.innerText = _Symbol.First; });
        $div.filter("div[id=Pager_Prev]").each(function () { this.innerText = _Symbol.Prev; });
        $div.filter("div[id=Pager_Next]").each(function () { this.innerText = _Symbol.Next; });
        $div.filter("div[id=Pager_Last]").each(function () { this.innerText = _Symbol.Last; });
    }
    else {
        $("div[id=Pager_First]").each(function () { this.innerText = _Pager.First; });
        $("div[id=Pager_Prev]").each(function () { this.innerText = _Pager.Prev; });
        $("div[id=Pager_Next]").each(function () { this.innerText = _Pager.Next; });
        $("div[id=Pager_Last]").each(function () { this.innerText = _Pager.Last; });    
    }
    
    $("span[id=StrPager1]").html(_StrPager.StrPager1);
    $("span[id=StrPager2]").html(_StrPager.StrPager2);
    $("span[id=StrPager3]").html(_StrPager.StrPager3);
    $("span[id=StrPager4]").html(_StrPager.StrPager4);
    $("span[id=StrPager5]").html(_StrPager.StrPager5);
    $("input[id=PageSize]").attr("titlename", _PageSizeTitle);
}

function _Pager_onChange(srcElement) {
    eval("if(typeof(SetValidate)==='function'){ SetValidate(); }");
    var Result = _FormValidation();
    if (Result) {
        var Id = $(srcElement).attr("id");
        var PageIndex = $("select[id=PageIndex]").val();
        var Last = $("select[id=PageIndex] option").length;

        PageIndex = parseInt(PageIndex * 1, 10);

        switch (Id) {
            case "Pager_First": { PageIndex = 1; break; }
            case "Pager_Prev": { PageIndex = PageIndex - 1; break; }
            case "Pager_Next": { PageIndex = PageIndex + 1; break; }
            case "Pager_Last": { PageIndex = Last; break; }
        }
        if (PageIndex <= 0) {
            PageIndex = 1;
        } else {
            if (PageIndex > Last) {
                PageIndex = Last;
            }
        }
        $("select[id='PageIndex']").val(PageIndex);
        var formElement = _GetParentElementByTag(srcElement, "form");
        formElement.submit();
    }
    return false;
}

function _PageIndex_onChange(srcElement) {
    eval("if(typeof(SetValidate)==='function'){ SetValidate(); }");

    var Result = _FormValidation();
    var ID = $(srcElement).attr("id");
    $("#ExecAction").val(_ActionTypeSelect);
    if (Result) {
        eval("if(typeof(" + ID + "_onChange)==='function'){ Result = " + ID + "_onChange(srcElement); }");
    }
    if (Result) {
        var formElement = _GetParentElementByTag(srcElement, "form");
        formElement.submit();
    }
    return true;
}
//#endregion

//#region TabStrip共用
function _TabStrip_onClick(srcElement) {
    var handleing = true;
    
    eval("if(typeof(" + srcElement.id + "_onClick)=='function') { handleing = " + srcElement.id + "_onClick(srcElement); }");
    
    if (handleing != true) {
        handleing = false;
        window.event.returnValue = false;
    };

    return handleing;
}
//#endregion

//#region Tabs共用
function _Tabs_onClick(srcElement, key) {
    srcElement = srcElement[0];
    var keys = key.split(";");
    var handleing = true;
    var con = $(srcElement).attr("con");
    var act = $(srcElement).attr("act");
    var sys = $(srcElement).attr("sys");
    var action = "/" + con + "/" + act;

    if (sys) {
        action = sys + action;
    }

    eval("if(typeof(" + srcElement.id + "_onClick)=='function') { handleing = " + srcElement.id + "_onClick(srcElement,keys); } else { window.location.href = action; }");
    
    return handleing;
}
//#endregion

//#region BasicCode 共用
function _BasicCode_onBlur(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";
    var value = $(srcElement).val();
    
    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (Result) {
        eval("if(typeof(" + ElementId + "_onBlur)==='function'){ Result = " + ElementId + "_onBlur(srcElement); }");
    }
    
    if (value != srcElement.defaultValue) {
        $(srcElement).addClass("Changed");
    } else {
        $(srcElement).removeClass("Changed");
    }

    return Result;
}

function _BasicCode_onKeyPress(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";

    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (Result) {
        eval("if(typeof(" + ElementId + "_onKeyPress)==='function'){ Result = " + ElementId + "_onKeyPress(srcElement); }");

        if (Result && event.keyCode == 13) {
            Result = _BasicCode_onEnter(srcElement);
        }
    }

    return Result;
}

function _BasicCode_onEnter(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";

    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (Result) {
        eval("if(typeof(" + ElementId + "_onEnter)==='function'){ Result = " + ElementId + "_onEnter(srcElement); }");
    }

    return Result;
}

function _BasicCode_onClick(srcElement, equalname) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";
    var rowName = "";
    
    var splitID = ID.split(".");
    ElementId = splitID[splitID.length - 1];

    if (ID.split(".").length > 1) {
        rowName = ID.split(".")[0];
    }

    if (Result) {
        eval("if(typeof(" + ElementId + "_onClick)==='function'){ Result = " + ElementId + "_onClick(srcElement, equalname,'" + rowName + "'); }");
    }
    return Result;
}
//#endregion

//#region Interval 共用
function _Interval_onBlur(nameF, nameT) {
    _Interval(nameF, nameT);
}
function _Interval(nameF, nameT) {
    var srcElementF = $("#" + nameF.replace(/[\[]/g, "\\[").replace(/[\]]/g, "\\]").replace(/[\.]/g, "\\."));
    var srcElementT = $("#" + nameT.replace(/[\[]/g, "\\[").replace(/[\]]/g, "\\]").replace(/[\.]/g, "\\."));
    if ($.trim(srcElementF.val()) == "" && $.trim(srcElementT.val()) != "") {
        srcElementF.val(srcElementT.val());
    }
    if ($.trim(srcElementT.val()) == "" && $.trim(srcElementF.val()) != "") {
        srcElementT.val(srcElementF.val());
    }
}
//#endregion


/* 公用function ------------------------------------------------------------ */

//#region post 導頁
function _submit(srcElement) {
    srcElement = srcElement[0];
    var formElement = _GetParentElementByTag(srcElement, "form");
    var con = $(srcElement).attr("con");
    var act = $(srcElement).attr("act");
    var action = "/" + con + "/" + act;
    $("#" + formElement.id).attr("action", action);
    $.blockUI({ message: '' });
    formElement.submit();
}
//#endregion

//#region 另開視窗
function _openWin(windowName, url, parameters, extendFeatures) {
    document.domain = _domain;
    var width = 600, height = 400;
    var left = 10, top = 10;
    if (window.screen) {
        width = window.screen.availWidth * 70 / 100;
        height = window.screen.availHeight * 70 / 100;

        left = (window.screen.availWidth - 10 - width) / 2;
        top = (window.screen.availHeight - 30 - height) / 2;
    }

    var objfeatures = { location: 0, menubar: 0, resizable: 1, scrollbars: 1, status: 0, titlebar: 0, toolbar: 0,
        width: width, height: height, top: top, left: left
    };

    if (extendFeatures != undefined) {
        $.extend(objfeatures, extendFeatures);
    }

    var features = '';
    if (extendFeatures !== null) {
        $.each(objfeatures, function (key, value) {
            features += key + '=' + value + ',';
        });
        features = features.substring(0, features.length - 1);
    }

    var targetWinName = windowName + "LionTech_blank";
    if (windowName === null ||
        windowName === undefined) {
        targetWinName += '_' + new Date().toLocaleString().replace(/\/|[\u4e00-\u9fa5]|:|\s+/g, "");
    }

    var newWindow = window.open(_enumERPAP + '/Pub/Empty', targetWinName, features, true);

    var tmpForm =
        $(document.createElement('form')).attr({
            target: targetWinName,
            method: 'post',
            action: url
        });

    tmpForm.append("<input type='hidden' name='OpenWin' value='1'>");

    if (parameters !== undefined &&
        parameters !== null) {
        var strQuery = parameters.split('&');
        for (var i = 0; i < strQuery.length; i++) {
            var strTemp = strQuery[i].split('=');
            if (strTemp.length === 2) {
                tmpForm.append("<input type='hidden' name='" + strTemp[0] + "' value='" + strTemp[1] + "'>");
            }
        }
    }

    $('body').append(tmpForm);
    tmpForm[0].submit();

    setTimeout(function() {
        tmpForm.remove();
    }, 1000);

    return newWindow;
}
//#endregion

//#region post 檔案下載
function _fileDownload(formElement, action_url, removeTime, functionName) {
    var Result = true;
    
    //留存form原有的action
    var original_url = $(_formElement).attr('action');

    //建立一組iframe
    var iframe = document.createElement("iframe");
    iframe.setAttribute("id", "download_iframe");
    iframe.setAttribute("name", "download_iframe");
    iframe.setAttribute("width", "0");
    iframe.setAttribute("height", "0");
    iframe.setAttribute("border", "0");
    iframe.setAttribute("style", "width: 0; height: 0; border: none;");

    //將iframe加入至document
    formElement.parentNode.appendChild(iframe);
    window.frames['download_iframe'].name = "download_iframe";
    iframeId = document.getElementById("download_iframe");

    //設定form屬性
    formElement.setAttribute("target", "download_iframe");
    formElement.setAttribute("action", action_url);
    formElement.setAttribute("method", "post");

    //Submit form
    formElement.submit();

    //重新設定form原有的屬性
    formElement.setAttribute("target", "_self");
    formElement.setAttribute("action", original_url);

    //刪除iframe
    setTimeout('iframeId.parentNode.removeChild(iframeId)', removeTime);

    //後續處理轉發呼叫
    eval("if(typeof(" + functionName + ")==='function') { Result = " + functionName + "(); }");
    return Result;
}
//#endregion

//#region 代碼help
function QSData() {
    var vMapFields;
}
function Ini_decData() {
    _decData.vMapFields = "";
    return 1;
}
function _hISearch(mapFields, windowName, url, parameters, extendFeatures) {
    Ini_decData();
    _decData.vMapFields = mapFields;
    _openWin(windowName, url, parameters, extendFeatures);
}
function _hIChanged(srcElement) {
    var Result = true;
    var value = $(srcElement).val();
    if (value != srcElement.defaultValue) {
        $(srcElement).addClass("Changed");
    } else {
        $(srcElement).removeClass("Changed");
    }
    return Result;
}
//#endregion

//#region Table Hover共用
function _TableHover(srcElement, formElement) {
    $('#' + srcElement + ' tr', formElement).off('hover');
    $('#' + srcElement + ' tr', formElement).off('click');

    $('#' + srcElement + ' tr', formElement).hover(
        function () {
            $(this).addClass('tr-hover');
        }, function () {
            $(this).removeClass('tr-hover');
        }
    ).click(function () {
        if ($(this).hasClass('tr-selected')) {
            $(this).removeClass('tr-selected');
        } else {
            $(this).addClass('tr-selected');
        }
    });
}
//#endregion

//#region 設定Original值
function _SetFormOriginalValue(form, inputID) {
    var obj = form.getElementsByTagName("INPUT")[inputID];
    if (obj == undefined) { obj = form.getElementsByTagName("SELECT")[inputID]; }
    if (obj != undefined) {
        if (obj.tagName == "SELECT") {
            if (obj.selectedIndex == -1) {
                obj.setAttribute("originalvalue", "");
            } else {
                obj.setAttribute("originalvalue", obj.value);
            }
        } else if (obj.type == "text") {
            obj.setAttribute("originalvalue", obj.value);
        } else if (obj.type == "radio" || obj.type == "checkbox") {
            if (obj.checked == true) {
                obj.setAttribute("originalvalue", "checked");
            } else {
                obj.setAttribute("originalvalue", "");
            }
        }
    }
    return;
}
//#endregion

//#region 抓上一層tag
function _GetParentElementByTag(obj, sTag) {
    obj = obj.parentNode;
    while (obj.tagName != sTag.toUpperCase()) {
        obj = obj.parentNode;
        if (obj == null) {
            return null;
        }
    }
    return obj;
}
//#endregion   

//#region 設定input值或combo的index
function _SetFormInputValue(form, inputID, value) {
    var obj = form.getElementsByTagName("INPUT")[inputID];
    if (obj == undefined) { obj = form.getElementsByTagName("SELECT")[inputID]; }
    if (obj != undefined) {
        if (obj.tagName == "SELECT") {
            obj.selectedIndex = -1;
            if (obj.options.length > 0) {
                for (var j = 0; j < obj.options.length; j++) {
                    if (obj.options[j].value == value) {
                        obj.selectedIndex = j;
                        break;
                    }
                }
            }
        } else if (obj.type == "text" || obj.type == "hidden") {
            obj.value = value;
        } else if (obj.type == "radio" || obj.type == "checkbox") {
            if (obj.value == value) {
                obj.checked = true;
            } else {
                obj.checked = false;
            }
        }

    }
    return;
}
//#endregion

//#region 小月曆
function _Datepicker(formElement) {
    $("[inputtype='TextBoxDatePicker']", formElement).datepicker({
        showOn: "button",
        dateFormat: "yymmdd",
        changeMonth: true,
        changeYear: true,
		beforeShow: function() {
			setTimeout(function() {
				$('.ui-datepicker').css('z-index', 3 + parseInt($('.ui-datepicker').css('z-index')));
			}, 0);
		}
    }).next('button').addClass('helper calendar');
}
//#endregion

//#region 錯誤訊息與驗證

//#region 設定錯誤訊息框的資料
function _SetErrMessageBox() {
//    var ErrMessage = "";
//    $("[validation]").each(function () {
//    	ErrMessage += "<label " + _JsErrMessageBoxLabel + "='" + _JsErrMessageBoxLabel + "' for='" + $(this).attr('id') + "' id=" + _Err + $(this).attr('id') + " style='display:none;' " + _display + " = '" + _display + "'>" + $(this).attr("title") + " <span name='othermsg'></span><br></label>"
//    });

//    $("#" + _JsErrMessageBoxLabel).html(ErrMessage);
    $("#CloseJsErrMessageBox").val(_CloseJsErrMessage);
    $("#JsErrMessageBoxTitle").html(_JsErrMessageBoxTitle);
    $("#" + _JsErrMessageBox).draggable();
}
//#endregion

//#region 整個form 驗證
function _FormValidation() {
    var Result = true;
    $("[validation]").each(function () {
        var inputType = $(this).attr('inputtype');
        var value = $(this).val();
        var Validate = true;
        var labelId = "#" + _Err + $(this).attr("id").replace(/\[/g, "\\[").replace(/\]/g, "\\]").replace(/\./g, "\\.");
        var tObj = $(labelId);

        Validate = _Validation(inputType, value, this, tObj);
        if (!Validate) {
            if (_isScriptOrOnEventInput) {
                _AddJsErrMessage(_JsErrMessageScriptOrOnEvent, $(this).attr("id"));
            } else {
                _AddJsErrMessage(_getTextBoxTitle(this), $(this).attr("id"));
            }
            Result = false;
        }
    });

    $('input:text,textarea').each(function () {
        var allowHTML = $(this).attr('AllowHTML');
        if (allowHTML == undefined && _IsMatchHTMLTag(this.value)) {
            $(this).val(encodeURIComponent(this.value));
            
            var inputType = $(this).attr('inputtype');
            var maxLength = $(this).attr("maxlength");
            var minlength = $(this).attr("minimumlength");
            Result = _CheckMaxLength(inputType, $(this).val(), maxLength, minlength, this);
            
            if (!Result) {
                if (_isScriptOrOnEventInput) {
                    _AddJsErrMessage(_JsErrMessageScriptOrOnEvent, $(this).attr("id"));
                } else {
                    _AddJsErrMessage(_getTextBoxTitle(this), $(this).attr("id"));
                }
            }
        }
    });

    if ($.trim($("#" + _JsOtherErrMessageBoxLabel).html()) > "") {
        Result = false;
    }

    _ShowJsErrMessageBox();
    return Result;
}
//#endregion

//#region 基本驗證 長度 inputType 必填
function _Validation(inputType, value, srcElement, tObj) {
    var Result = true;
    _isScriptOrOnEventInput = false;
    value = $.trim(value);
    $(srcElement).removeAttr("tmplength");
    if ($(srcElement).attr(_IsRequired) === _IsRequired) {
        Result = _CheckRequired(value, this);
    }
    if (inputType != undefined) {
        if (Result && $(srcElement).attr('allowHTML') == undefined) { Result = _CheckScriptOrOnEventInput(value); }
        if (Result) { Result = _CheckInputType(inputType, value, srcElement); }
        if (Result) {
            //tObj.find("span[name='othermsg']").html("");
            var maxLength = $(srcElement).attr("maxlength");
            var minlength = $(srcElement).attr("minimumlength");
            Result = _CheckMaxLength(inputType, $(srcElement).val(), maxLength, minlength, srcElement)
//            if (Result == false) {
//                var Resources = GetInputTypeResources(inputType);
//                tObj.find("span[name='othermsg']").html(_Currently + $(srcElement).attr("tmplength") + Resources.InputTypeCodeResource);
//            }
        }
    }
    return Result;
}
//#endregion

//#region 必填驗證
function _CheckRequired(value, srcElement) {
    if ($.trim(value) == "") {
        return false;
    }
    return true;
}
//#endregion

//#region 字串長度驗證
function _CheckMaxLength(inputType, value, maxLength, minlength, srcElement) {
    var tmplength = 0;
    
    if (maxLength == undefined) {
        maxLength = $(srcElement).attr("maxlength");
    }
    if (minlength == undefined) {
        minlength = $(srcElement).attr("minimumlength");
    }
    if (value.length == 0) {
        $(srcElement).attr("tmplength", tmplength);
        return true;
    }
    for (i = 0; i < value.length; i++) {
        if (value.charCodeAt(i) == 10) {
            tmplength += 2;
        } else {
            tmplength += 1;
        }
    }
    $(srcElement).attr("tmplength", tmplength);
    if (tmplength > maxLength || (minlength != undefined && tmplength < parseInt(minlength, 10)))
    { return false; } else { return true; }
}
//#endregion

//#region Interval驗證
function _CheckInterval(nameF, nameT) {
    var Result = true;
    var srcElementF = $("#" + nameF.replace(/[\[]/g, "\\[").replace(/[\]]/g, "\\]").replace(/[\.]/g, "\\."));
    var srcElementT = $("#" + nameT.replace(/[\[]/g, "\\[").replace(/[\]]/g, "\\]").replace(/[\.]/g, "\\."));
    if ($.trim(srcElementF.val()) > "" && $.trim(srcElementT.val()) > "") {
        if ($.trim(srcElementF.val()) > $.trim(srcElementT.val())) {
            Result = false;
        }
    }
    return Result
}
//#endregion

//#region idT物件有值idF物件必須輸入
function _CheckFirstIdRequired(idF, idT) {
    var Result = true;
    var srcElementF = $("#" + idF.replace(/[\[]/g, "\\[").replace(/[\]]/g, "\\]").replace(/[\.]/g, "\\."));
    var srcElementT = $("#" + idT.replace(/[\[]/g, "\\[").replace(/[\]]/g, "\\]").replace(/[\.]/g, "\\."));
    if ($.trim(srcElementF.val()) == "" && $.trim(srcElementT.val()) != "") {
        Result = false;
    }
    return Result
}
//#endregion

//#region textbox 資料型態驗證
function _CheckScriptOrOnEventInput(value) {
    var express = /(<script)|(<((["][^"]*["])|(['][^']*['])|[^'"])*(\bon\B))/i;
    if (express.test(value)) {
        _isScriptOrOnEventInput = true;
        return false;
    }

    return true;
}

function _IsMatchHTMLTag(value) {
    var express = /<[A-Za-z/]+/;
    if (express.test(value)) {
        return true;
    }

    return false;
}

function _CheckInputType(inputType, value, srcElement) {
    var Result = true;
    if (value > "") {
        switch (inputType) {
            case "TextBoxAlphanumeric":
                {
                    var alphanumericExpression = /^[A-Za-z0-9]*$/;
                    Result = alphanumericExpression.test(value);
                    break;
                }
            case "TextBoxDecimal":
                {
                    value = value.replace(/(^\s*)|(\s*$)/g, '');
                    var integerExpression = /^([-]?([0-9]*)(\.([0-9]+))?)$/;
                    var maximum = $(srcElement).attr('maximum');
                    var minimum = $(srcElement).attr('minimum');
                    Result = integerExpression.test(value);
                    if (Result) { Result = _CheckRange(value, maximum, minimum, srcElement) }
                    break;
                }
            case "TextBoxInteger":
                {
                    value = value.replace(/(^\s*)|(\s*$)/g, '');
                    var integerExpression = /^(\-)?\d*$/;
                    var maximum = $(srcElement).attr('maximum');
                    var minimum = $(srcElement).attr('minimum');
                    Result = integerExpression.test(value);
                    if (Result) { Result = _CheckRange(value, maximum, minimum, srcElement) }
                    break;
                }
            case "TextBoxNotChinese":
                {
                    var TextBoxNotChineseExpression = /[\u4e00-\u9fa5]/;
                    for (i = 0; i < value.length; i++) {
                        tmpchar = value.substr(i, 1);
                        if (TextBoxNotChineseExpression.test(tmpchar)) {
                            Result = false;
                            break;
                        }
                    }
                    break;
                }
               case "TextBoxChar8":
                {
                    if (value.length == 4) {
                        value = (new Date()).getFullYear() + value;
                        $(srcElement).val(value);
                    }
                    Result = _CheckDates(value,srcElement);
                    break;
                }
            case "TextBoxDatePicker":
                {
                    if (value.length == 4) {
                        value = (new Date()).getFullYear() + value;
                        $(srcElement).val(value);
                    }
                    Result = _CheckDates(value,srcElement);
                    break;
                }
            case "TextBoxNumber":
                {
                    value = value.replace(/(^\s*)|(\s*$)/g, '');
                    var integerExpression = /^[0-9]*$/;
                    var maximum = $(srcElement).attr('maximum');
                    var minimum = $(srcElement).attr('minimum');

                    Result = integerExpression.test(value);
                    if (Result) { Result = _CheckRange(value, maximum, minimum, srcElement) }
                    break;
                }
            case "TextBoxIdNo":
                {
                    Result = _ChkIdNo(value, srcElement);
                    break;
                }
            case "TextBoxInterval":
                {
                    if (value.length == 4) {
                        value = (new Date()).getFullYear() + value;
                        $(srcElement).val(value);
                    }
                    Result = _CheckDates(value,srcElement);
                    break;
                }
            case "TextBoxYearMonth":
                {
                    Result = _CheckYm(value, srcElement);
                    break;
                }
        }
    }

    return Result;
}
//#endregion

//#region 數值區間驗證
function _CheckRange(value, maximum, minimum, srcElement) {
    value = parseFloat(value);
    if (maximum != undefined && minimum != undefined) {
        maximum = parseFloat(maximum);
        minimum = parseFloat(minimum);
        if (value > maximum || value < minimum) {
            return false;
        }
    }
    return true;
}
//#endregion

//#region char8驗證
function _CheckDates(value,srcElement) {
    var IsPass = true;
    var tYear, tMm, tDd;
    var tFlag = 0; //1 潤年
    var tChkstr = /[^0-9]/;
    var tvalue = value;
    if (tChkstr.test(tvalue)) { IsPass = false; }
    if (value.length != 8) IsPass = false;

    if (IsPass == true) {
        tvalue = value;
        tYear = parseInt(tvalue.substr(0, 4), 10);
        tMm = parseInt(tvalue.substr(4, 2), 10);
        tDd = parseInt(tvalue.substr(6, 2), 10);

        if (tYear < 1753 || tMm < 1 || tMm > 12 || tDd < 1 || tDd > 31)
        { IsPass = false; }
        else {
            if (tYear % 4 == 0)	//潤年
            {
                tFlag = 1;
                if (tYear % 100 == 0)
                    if (tYear % 400 != 0) tFlag = 0;
            }
            switch (tMm) {
                case 2: if ((tFlag == 1 && tDd > 29) || (tFlag == 0 && tDd > 28)) IsPass = false; break;
                case 4:
                case 6:
                case 9:
                case 11: if (tDd > 30) IsPass = false; break;
            }
        }
    }

    return (IsPass);

}
//#endregion

//#region 年月驗證
function _CheckYm(value, srcElement) {
    var IsPass = true;
    var tYear, tMm;
    var tChkstr = /[^0-9]/;

    if (tChkstr.test(value)) IsPass = false;
    if (value.length != 6) IsPass = false;

    if (IsPass) {
        tYear = parseInt(value.substr(0, 4), 10);
        tMm = parseInt(value.substr(4, 2), 10);
        if (tYear < 1 || tMm < 1 || tMm > 12)
            IsPass = false;
    }
    return IsPass;
}
//#endregion

//#region 驗證身份證
function _ChkIdNo(value, srcElement) {
    var a = new Array('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'X', 'Y', 'W', 'Z', 'I', 'O');
    var b = new Array(1, 9, 8, 7, 6, 5, 4, 3, 2, 1);
    var c = new Array(2);
    var d;
    var e;
    var f;
    var g = 0;
    var h = /^[a-z](1|2|8|9)\d{8}$/i;
    if ($.trim(value) == "") {
        return true;
    }
    if (value.search(h) == -1) {
        return false;
    }
    else {
        d = value.charAt(0).toUpperCase();
        f = value.charAt(9);
    }
    for (var i = 0; i < 26; i++) {
        if (d == a[i])//a==a
        {
            e = i + 10; //10
            c[0] = Math.floor(e / 10); //1
            c[1] = e - (c[0] * 10); //10-(1*10)
            break;
        }
    }
    for (var i = 0; i < b.length; i++) {
        if (i < 2) {
            g += c[i] * b[i];
        }
        else {
            g += parseInt(value.charAt(i - 1)) * b[i];
        }
    }

    if ((parseInt(g) + parseInt(f)) % 10 == 0) {
        return true;
    }
    else {
        return false;
    }
}
//#endregion

//#region show 錯誤訊息
function _ShowJsErrMessageBox() {
    var show = false;
    var tObj = $("#" + _JsErrMessageBox);
    if (tObj.find("label[" + _display + " ='']").length > 0) {
        show = true;
        //$("div").find("label[" + _display + " ='']").eq(0).trigger('click');
    }
    if (show) {
        tObj.show();
        tObj.css("top", Math.max(0, (($(window).height() - tObj.outerHeight()) / 2) +
                                                $(window).scrollTop()) + "px");
        tObj.css("left", Math.max(0, (($(window).width() - tObj.outerWidth()) / 2) +
                                                $(window).scrollLeft()) + "px");
        tObj.css("z-index", 3);
    } else {
        tObj.hide();
    }
}
//#endregion

//#region 關閉錯誤訊息
function _ClossJsErrMessageBox() {
    var tObj = $("#" + _JsErrMessageBox);
    //tObj.find("label").hide().attr(_display, _display);
    //$("div[id='" + _JsErrMessageBox + "'] label").hide().attr(_display, _display);
    //$("div[id='" + _JsErrMessageBox + "']").hide();
    $("#" + _JsOtherErrMessageBoxLabel).html("");
    tObj.hide();
}
//#endregion

//#region 讓pg自己寫錯誤訊息 
//需寫在自己訂的function內,例function SetValidate {_AddJsErrMessage("error");}
function _AddJsErrMessage(ErrMessage, forname) {
    ErrMessage = "<label " + _JsOtherErrMessageBoxLabel + "='" + _JsOtherErrMessageBoxLabel + "' " + (forname > "" ? "for='" + forname + "' id = '" + _Err + forname + "' style='cursor:pointer'" : "") + _display + " = ''>" + ErrMessage + "<br></label>";
    $("#" + _JsOtherErrMessageBoxLabel).html($("#" + _JsOtherErrMessageBoxLabel).html() + ErrMessage);
}
//#endregion

//#endregion

//#region 設定document title
function _SetDocumentTitle(title) {
    $(document).attr("title", title);
}
//#endregion

//#region 設定input title
function _setTitle() {
    $("[title]").each(function (i,v) {
        $(this).attr("otitle", $(this).attr("title"));
    });
    $("[titlename]").each(function () {
        $(this).attr("title", _getTextBoxTitle(this));
    });
}
//#endregion

//#region 組合combo跟textbox的title
function _getTextBoxTitle(srcElement) {
    var inputType = $(srcElement).attr('inputtype');
    var Resources = GetInputTypeResources(inputType);
    var minlength = $(srcElement).attr('minimumlength');
    var maximum = $(srcElement).attr('maximum');
    var minimum = $(srcElement).attr('minimum');
    var basicCodeType = $(srcElement).attr("BasicCodeType");
    var InputTitle = "";

    if ($(srcElement).attr("titlename") != undefined) {
        InputTitle = $(srcElement).attr("titlename") ;
        if (inputType != undefined) {
            InputTitle += "：" + Resources.InputTypeResource;
            if (inputType == "TextBoxChar8" || inputType == "TextBoxDatePicker" || inputType == "TextBoxInterval" || inputType == "TextBoxYearMonth") {
                InputTitle +=  Resources.InputTypeCodeResource;
            } else if (inputType == "TextBoxInteger" || inputType == "TextBoxNumber" || inputType == "TextBoxDecimal") {
                if (maximum != undefined && minimum != undefined) {
                    InputTitle += " " + minimum + "~" + maximum;
                }
            } else {
                InputTitle += $(srcElement).attr("maxlength") + Resources.InputTypeCodeResource
            }
            if (minlength != undefined) {
                InputTitle += " " + _Least + minlength + Resources.InputTypeCodeResource;
            }
        } 
        if (basicCodeType != undefined) {
            InputTitle += "：" + GetBasicCodeResources(basicCodeType);
        }
        if ($(srcElement).attr(_IsRequired) === _IsRequired) {
            if ($(srcElement)[0].tagName == "select") {
                InputTitle += "：" + Resources.Select;
            } else {
                InputTitle += " " + Resources.Required;
            }
        }
    } else {
        InputTitle = $(srcElement).attr("otitle");
    }
    if ($(srcElement).attr("tmplength") != undefined) {
        InputTitle += " " + (_Currently + $(srcElement).attr("tmplength") + Resources.InputTypeCodeResource);
    }
    return InputTitle;
}
//#endregion

//#region alert訊息
function _alert(ID, message,width) {
    var originalvalue ;
    $("div #MessageBoxAlert").each(function(){
        if (_GetParentElementByTag(this, 'DIV').id == ID) {
            originalvalue = $("#MessageBoxAlert").html();
        }
    });
    if (width == undefined) {width = 400; }
    if (message != undefined) {
        $("div #MessageBoxAlert").each(function(){
            if (_GetParentElementByTag(this, 'DIV').id == ID) {
                $(this).html(message);
            }
        });
    }
    $.blockUI({ message: $('#' + ID).html(), 
        css: {
            padding: 0,
            margin: 0,
            width: (width + 'PX'),
            top: Math.max(0, (($(window).height() - $("#" + ID).outerHeight()) / 2)) + "px",
            left: Math.max(0, (($(window).width() - ($("#" + ID).outerWidth() > width ? width : $("#" + ID).outerWidth())) / 2)) + "px",
            textAlign: 'center',
            cursor: 'wait'
        }
    });
    $("div #MessageBoxAlert").each(function(){
        if (_GetParentElementByTag(this, 'DIV').id == ID) {
            $("#MessageBoxAlert").html(originalvalue);
        }
    });
    return true;
}
//#endregion

//#region unblockUI
function _btnUnblockUI(srcElement, returnValue) {
    $.unblockUI();
    var ID = $(srcElement).attr('id');
    var Result = returnValue;
    if (Result) {
        eval("if(typeof(" + ID + "_onClick)==='function'){ Result = " + ID + "_onClick(srcElement); }");
    }
    return Result;
}
//#endregion

//#region window.Close
function _windowClose() {
    window.opener = null;
    window.open("", "_self");
    window.close();
}
//#endregion

//#region 新增 刪除 複製行
//chkNM 第一個控制項名字
//name*= chkNM 第一個控制項

//addTR 新增行，用於新增行按鈕
function _addTR(chkNM) {
    var rows = $("[name*='" + chkNM + "']");
    var ch = rows.last();
    var ntr = _getNewTR(ch, rows.length);
    //新增行時清空值
    ntr.find("input:text,textarea,select").each(function (idx, item) {
        $(item).val("");
    });
    ntr.find("input:checkbox,input:radio").each(function (idx, item) {
        $(item).attr("checked", false);
    });
}

//copyTR 複製行，用於複製行按鈕
function _copyTR(chkNM) {
    $("[name*='" + chkNM + "']").each(function () {
        if ($(this).is(":checked")) {
            var rows = $("[name*='" + chkNM + "']");
            var selects = $(this).closest("tr").find("select");
            var texts = $(this).closest("tr").find("input:text,textarea");
            var checkboxs = $(this).closest("tr").find("input:checkbox,input:radio");
            var ntr = _getNewTR(this, rows.length);
            ntr.find("select").each(function (idx, item) {
                $(item).val(selects.eq(idx).val());
            });
            ntr.find("input:text,textarea").each(function (idx, item) {
                $(item).val(texts.eq(idx).val());
            });
            ntr.find("input:checkbox,input:radio").each(function (idx, item) {
                $(item).attr("checked", false);
                if (checkboxs.eq(idx).is(":checked")) {
                    $(item).attr("checked", "checked");
                }
            });
        }
    });
}

//deleteTR 刪除行，用於刪除行按鈕
function _deleteTR(chkNM) {
    $("[name*='" + chkNM + "']").each(function (idx) {
        if ($(this).is(":checked") && $("[name*='" + chkNM + "']").length > 1) {
            $(this).closest("tr").remove();
        }
    });

    $("[name*='" + chkNM + "']").each(function (idx) {
        $(this).closest("tr").find("[id*='['],[name*='[']").each(function () {
            var tname = $(this).attr('name');
            var tid = $(this).attr('id');
            var tonClick = $(this).attr('onclick');
            if (tname != undefined) {
                tname = _reName(tname, idx);
                $(this).attr('name', tname);
            }
            if (tid != undefined) {
                tid = _reName(tid, idx);
                $(this).attr('id', tid);
            }
            if (tonClick != undefined) {
                tonClick = _reName(tonClick, idx);
                $(this).attr('onclick', tonClick);
            }
        })
    });
}

function _getNewTR(obj, rowcount) {
    var ch = $(obj);
    var tb = ch.closest("table");
    var tr = ch.closest("tr");
    var ntr = tr.clone();
    var thtml = $(ntr).html();
    thtml = _reName(thtml, rowcount);
    ntr.html(thtml);
    tb.append(ntr);
    return ntr;
}

function _reName(ihtml, idx) {
    return ihtml.replace(/\[\d+\]/g, "[" + idx + "]");
}
//#endregion

//#region 加掛編輯器
function _ckeditor(ElementID) {
    $("textarea[id='" + ElementID + "']").addClass("ckeditor");
}
//#endregion

//#region 設定數值格式
function _GetNumber(value) {
    var number;
    number = value.replace(/(^\s*)|(\s*$)/g, '');
    number = number.replace(/,/g, '');
    var moneyExpression = /^[0-9-.]*$/;
    if (number == '' || moneyExpression.test(number) == false || isNaN(number * 1) == true) {
        number = null;
    } else {
        number = number * 1;
    }
    return number;
}

//取得整數格式
function _FormatNumberToInteger(value) {
    var number = _GetNumber(value.toString());
    if (number == null) {
        return "";
    } else if (number.toString().indexOf('.') > -1) {
        return "";
    } else {
        return number.toString();
    }
}

//取得金額格式
function _FormatNumberToMoney(value) {
    var number = _GetNumber(value.toString());
    if (number == null) {
        return "";
    } else {
        number = number.toString();
        var commaExpression = new RegExp('(-?[0-9]+)([0-9]{3})');
        var tmpValue = '';
        if (number.indexOf('.') > -1) {
            tmpValue = number.substr(number.indexOf('.'), number.length - number.indexOf('.'));
            number = number.substr(0, number.indexOf('.'));
        }
        while (commaExpression.test(number)) {
            number = number.replace(commaExpression, '$1,$2');
        }
        if (tmpValue.toString() == '') {
            tmpValue = '.00';
        } else {
            tmpValue = tmpValue.toString() + '00';
        }
        return number.toString() + tmpValue.substr(0, 3);
    }
}

//取得整數金額格式
function _FormatNumberToIntegerMoney(value) {
    var number = _GetNumber(value.toString());
    if (number == null) {
        return "";
    } else {
        number = number.toString();
        var commaExpression = new RegExp('(-?[0-9]+)([0-9]{3})');
        var tmpValue = '';
        if (number.indexOf('.') > -1) {
            number = number.substr(0, number.indexOf('.'));
        }
        while (commaExpression.test(number)) {
            number = number.replace(commaExpression, '$1,$2');
        }
        return number.toString();
    }
}

//取得浮點數格式
function _FormatFloat(value, pos) {
    var number = _GetNumber(value.toString());
    var size = Math.pow(10, pos);
    return Math.round(number * size) / size;
}
//#endregion

//#region 字串組裝工具
String.format = function () {
    var s = arguments[0];
    if (s == null) return "";
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = _getStringFormatPlaceHolderRegEx(i);
        s = s.replace(reg, (arguments[i + 1] == null ? "" : arguments[i + 1]));
    }
    return _cleanStringFormatResult(s);
}

String.prototype.format = function () {
    var txt = this.toString();
    for (var i = 0; i < arguments.length; i++) {
        var exp = _getStringFormatPlaceHolderRegEx(i);
        txt = txt.replace(exp, (arguments[i] == null ? "" : arguments[i]));
    }
    return _cleanStringFormatResult(txt);
};


//讓輸入的字串可以包含{}
function _getStringFormatPlaceHolderRegEx(placeHolderIndex) {
    return new RegExp('({)?\\{' + placeHolderIndex + '\\}(?!})', 'gm');
}

//當format格式有多餘的position時，就不會將多餘的position輸出
//ex:
// var fullName = 'Hello. My name is {0} {1} {2}.'.format('firstName', 'lastName');
// 輸出的 fullName 為 'firstName lastName', 而不會是 'firstName lastName {2}'
function _cleanStringFormatResult(txt) {
    if (txt == null) return "";
    return txt.replace(_getStringFormatPlaceHolderRegEx("\\d+"), "");
}
//#endregion

//#region 凍結窗格
function _Scrolify(tblAsJQueryObject, height) {
    var oTbl = tblAsJQueryObject;
    var oTblDiv = $("<div id='tbodyDiv'></div>");

    oTbl.attr("data-item-original-width", oTbl.width());
    oTbl.find('thead tr th').each(function () {
        $(this).attr("data-item-original-width", $(this).width());
    });
    oTbl.find('tbody tr:eq(0) td').each(function () {
        $(this).attr("data-item-original-width", $(this).width());
    });

    oTblDiv.css('height', height);
    oTblDiv.css('overflow-y', 'scroll');
    oTbl.css('style', 'display:inline');
    oTbl.wrap(oTblDiv);

    var newTbl = oTbl.clone();
    newTbl.attr("id","theadTable");

    oTbl.find('thead tr').remove();
    newTbl.find('tbody tr').remove();

    oTbl.parent().parent().prepend(newTbl);
    newTbl.wrap("<div/>");

    newTbl.width(newTbl.attr('data-item-original-width'));
    newTbl.find('thead tr th').each(function () {
        $(this).width($(this).attr("data-item-original-width"));
    });
    oTbl.width(oTbl.attr('data-item-original-width'));
    oTbl.find('tbody tr:eq(0) td').each(function () {
        $(this).width($(this).attr("data-item-original-width"));
    });
}
//#endregion

//#region 停用/啟用凍結窗格
function _ToggleScrolify(tblAsJQueryObject, height) {
    var obj = $("div").find('#tbodyDiv');

    if (obj.length > 0 && obj.length == 1) {
        $(obj).contents().unwrap();
        tblAsJQueryObject.css('margin-bottom', '5px');
    }
    else {
        tblAsJQueryObject.css('margin-bottom', '0px');
        
        tblAsJQueryObject.attr("data-item-original-width", tblAsJQueryObject.width());
        tblAsJQueryObject.find('thead tr th').each(function () {
            $(this).attr("data-item-original-width", $(this).width());
        });
        tblAsJQueryObject.find('tbody tr:eq(0) td').each(function () {
            $(this).attr("data-item-original-width", $(this).width());
        });

        var tblDiv = $("<div id='tbodyDiv' style='margin-bottom:5px'></div>");
        tblDiv.css('height', height);
        tblDiv.css('overflow-y', 'scroll');
        tblAsJQueryObject.css('style', 'display:inline');
        tblAsJQueryObject.wrap(tblDiv);

         if($('#theadTable')[0]==null || $('#theadTable')[0]==undefined){
            var newTbl = tblAsJQueryObject.clone();
            newTbl.attr("id","theadTable");

            tblAsJQueryObject.find('thead tr').remove();
            newTbl.find('tbody tr').remove();

            tblAsJQueryObject.parent().parent().prepend(newTbl);
            newTbl.wrap("<div/>");
	
	        newTbl.width(newTbl.attr('data-item-original-width'));
            newTbl.find('thead tr th').each(function () {
                 $(this).width($(this).attr("data-item-original-width"));
             });
             tblAsJQueryObject.width($('#theadTable').attr("data-item-original-width"));
             tblAsJQueryObject.find('tbody tr:eq(0) td').each(function () {
                 $(this).width($(this).attr("data-item-original-width"));
             });
	    }
    }
}
//#endregion

//#region FunTool公用

function SysFunToolQueryButton_onClick(srcElement) {
    var formElement = $('form')[0];
    var toolNo = $('#FunToolNo').val();
    
    if (toolNo != '' && toolNo != undefined) {
        $.blockUI({ message: '' });

        $(formElement).append(
            $('<input>').attr({
                type: 'hidden',
                id: 'ToolNo',
                name: 'ToolNo',
                value: toolNo
            })
        );

        $.ajax({
            url: '/_BaseAP/GetUpdateSysFunToolResult',
            type: 'POST',
            data: { funControllerID: $('#FunToolControllerID').val(), funActionName: $('#FunToolActionName').val(), toolNo: toolNo },
            dataType: 'json',
            async: false,
            success: function (result) {
                if (result != null) {
                    $('#ExecAction').val(_ActionTypeQuery);
                    formElement.submit();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                _AddJsErrMessage(_JsMsg_UpdateSysFunToolError);
                _ShowJsErrMessageBox();
                _btnUnblockUI(this, false);
            }
        });
    }
    else {
        _AddJsErrMessage(_JsMsg_ToolNoIsRequired);
        _ShowJsErrMessageBox();
        _btnUnblockUI(this, false);
        return false;
    }
}

function SysFunToolUpdateButton_onClick(srcElement) {
    var formElement = $('form')[0];
    var toolNo = $('#FunToolNo').val();
    
    if (toolNo != '' && toolNo != undefined) {
        $.blockUI({ message: '' });

        $(formElement).append(
            $('<input>').attr({
                type: 'hidden',
                id: 'ToolNo',
                name: 'ToolNo',
                value: toolNo
            })
        );

        $('#ExecAction').val(_ActionTypeUpdate);
        formElement.submit();
    }
    else {
        _AddJsErrMessage(_JsMsg_ToolNoIsRequired);
        _ShowJsErrMessageBox();
        _btnUnblockUI(this, false);
        return false;
    }
}

function SysFunToolCreateButton_onClick(srcElement) {
    $.blockUI({ message: $('#SysFunToolCreateConfirmDialog') });
    
    $('#FunToolNo').val('');
    $('#FunToolNM').val('');

    return false;
}

function SysFunToolCreateConfirmOKButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    
    var formElement = $('form')[0];
    
    $(formElement).append(
        $('<input>').attr({
            type: 'hidden',
            id: 'ToolNM',
            name: 'ToolNM',
            value: $('#FunToolNM').val()
        })
    );

    $('#ExecAction').val(_ActionTypeAdd);

    formElement.submit();
}

function SysFunToolCreateConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

function SysFunToolCopyButton_onClick(srcElement) {
    var formElement = $('form')[0];
    var toolNo = $('#FunToolNo').val();

    $('#FunToolUserID_text').val('');
    $('#FunToolUserID').val('');

    if (toolNo != '' && toolNo != 'null' && toolNo != undefined) {
        $.blockUI({ message: $('#SysFunToolCopyConfirmDialog') });

        _NoFormAutoCompleteTextBox_GetList("FunToolUserID", "_BaseAP", "GetBaseRAWUserList");
    }
    else {
        _AddJsErrMessage(_JsMsg_ToolNoIsRequired);
        _ShowJsErrMessageBox();
    }

    return false;
}

function SysFunToolCopyConfirmOKButton_onClick(srcElement) {
    var isUseDefault;
    if ($('#IsUseDefault').prop("checked")) {
        isUseDefault = $('#IsUseDefault').val();
    }

    $.ajax({
        url: '/_BaseAP/GetCopySysFunToolResult',
        type: 'POST',
        data: {
            funControllerID: $('#FunToolControllerID').val(),
            funActionName: $('#FunToolActionName').val(),
            toolNo: $('#FunToolNo').val(),
            copyUserID: $('#FunToolUserID').val(),
            isUseDefault: isUseDefault
        },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $.blockUI({ message: $('#SysFunToolCopySuccessDialog') });
            }
            else {
                _AddJsErrMessage(_JsMsg_CopySysFunToolError);
                _ShowJsErrMessageBox();
                _btnUnblockUI(this, false);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(_JsMsg_CopySysFunToolError);
            _ShowJsErrMessageBox();
            _btnUnblockUI(this, false);
        }
    });
}

function SysFunToolCopyConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

function SysFunToolCopySuccessOKButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

function SysFunToolDeleteButton_onClick(srcElement) {
    var toolNo = $('#FunToolNo').val();

    if (toolNo != '' && toolNo != 'null' && toolNo != undefined) {
        $.blockUI({ message: $('#SysFunToolDeleteConfirmDialog') });
    }
    else {
        _AddJsErrMessage(_JsMsg_ToolNoIsRequired);
        _ShowJsErrMessageBox();
    }
    
    return false;
}

function SysFunToolDeleteConfirmOKButton_onClick(srcElement) {
    $.ajax({
        url: '/_BaseAP/GetDeleteSysFunToolResult',
        type: 'POST',
        data: { funControllerID: $('#FunToolControllerID').val(), funActionName: $('#FunToolActionName').val(), toolNo: $('#FunToolNo').val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#FunToolNo > option').each(function () {
                    if (this.value == result) {
                        $(this).remove();
                    }
                });

                $.blockUI({ message: $('#SysFunToolDeleteSuccessDialog') });
            }
            else {
                _AddJsErrMessage(_JsMsg_DeleteSysFunToolError);
                _ShowJsErrMessageBox();
                _btnUnblockUI(this, false);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(_JsMsg_DeleteSysFunToolError);
            _ShowJsErrMessageBox();
            _btnUnblockUI(this, false);
        }
    });
}

function SysFunToolDeleteConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

function SysFunToolDeleteSuccessOKButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

function SysFunToolUpdateNameButton_onClick(srcElement) {
    var funToolID = $('#FunToolNo').val();
    var funToolControllerID = $('#FunToolControllerID').val();
    var funToolActionName = $('#FunToolActionName').val();

    if (funToolID != "")
    {
        $.ajax({
            url: '/_BaseAP/GetSelectSysFunToolNameList',
            type: 'POST',
            data: { funToolControllerID:  funToolControllerID, funToolActionName: funToolActionName, toolNo: funToolID},
            dataType: 'json',
            async: false,
            success: function(result) {
                $('#FunToolUpdateName').val(result);
            },
            error: function(jqXHR, textStatus, errorThrown){
                _AddJsErrMessage(_JsMsg_UpdateNameSysFunToolError);
                _ShowJsErrMessageBox();
                _btnUnblockUI(this, false);
            }
        });

        $.blockUI({ message: $('#SysFunToolUpdateNameConfirmDialog') });
    }
    else
    {
        _AddJsErrMessage(_JsMsg_ToolNoIsRequired);
        _ShowJsErrMessageBox();
        _btnUnblockUI(this, false);
    }
}

function SysFunToolUpdateNameConfirmOKButton_onClick(srcElement) {
    var funToolID = $('#FunToolNo').val();
    var funToolNM = $('#FunToolUpdateName').val();
    var funToolControllerID = $('#FunToolControllerID').val();
    var funToolActionName = $('#FunToolActionName').val();
    
    $.ajax({
        url: '/_BaseAP/GetUpdateSysFunToolNameResult',
        type: 'POST',
        data: { funToolControllerID:  funToolControllerID, funToolActionName: funToolActionName, toolNo: funToolID, toolNM: funToolNM},
        dataType: 'json',
        async: false,
        success: function(result) {

            var toolNM = result;
            if(result != null) {
                $('#FunToolNo > option').each(function() {
                    if(this.value == funToolID) {
                        $(this).text(toolNM);
                    }
                });

                 $.blockUI({ message: $('#SysFunToolUpdateNameSuccessConfirmDialog') });
            } else {
                _AddJsErrMessage(_JsMsg_UpdateNameSysFunToolError);
                _ShowJsErrMessageBox();
                _btnUnblockUI(this, false);
            }
        },
        error: function(jqXHR, textStatus, errorThrown){
            _AddJsErrMessage(_JsMsg_UpdateNameSysFunToolError);
            _ShowJsErrMessageBox();
            _btnUnblockUI(this, false);
        }
    });
}

function SysFunToolUpdateNameConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

function SysFunToolUpdateNameSuccessOKButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}
//#endregion

function OpenWinOrderContactInfo(parameters) {
    var target = {
        OrderYear: 0,
        OrderSN: 0,
        OrderSeq: null,
        OrderKind: null,
        BookingSeq: null,
        OrderMyself: false,
        ProductType: null,
        SourceSysDomain: document.URL
    };

    var para = [];

    for (var i in parameters) {
        if (parameters.hasOwnProperty(i) && target.hasOwnProperty(i)) {
            para[para.length] = i + '=' + encodeURI(parameters[i]);
        }
    }

    para[para.length] = 'SourceSysDomain=' + encodeURI(document.URL);

    _hISearch(new Array(3), 'OrderContactInfo', _enumPUBAP + '/Order/OrderContactInfo', para.join('&'));
}

//#region WorkFlow公用
//WF 預設載入
$(function() {
    if ($('#WFNextNodeHasRole').val() === 'True' && $('table[id=WorkFlowDataBar] tr[id=WorkFlowAssginNextNodeNewUserBar]').length > 0) {
        _NoFormAutoCompleteTextBox_GetList("WFNextNodeNewUserID", "_BaseAP", "GetWFNextNodeRoleUserList");
    }
});

function WorkFlowToDoListButton_onClick(srcElement) {
    location.href = _enumWFAP + '/Pub/ToDoList';
    return true;
}

function WorkFlowSetSignatureButton_onClick(srcElement) {
    var objfeatures = { width: 700, height: 400 };
    var param = [];
    param[param.length] = 'WFNo=' + $('#WorkFlowData #WFWFNo').val();
    _openWin("workflowSetSignature", _enumWFAP + "/Pub/SetSignature", param.join('&'), objfeatures);
    return true;
}

function WorkFlowSignatureButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    _workflowAPIAction = 'Signature';

    _alert('WorkFlowConfirmDialog');
}

function WorkFlowDocumentButton_onClick(srcElement) {
    var param = [];
    param[param.length] = 'WFNo=' + $('#WorkFlowData #WFWFNo').val();
    _openWin('workflowDocument', _enumWFAP + '/Pub/Document', param.join('&'));
    return true;
}

function WorkFlowRemarkButton_onClick(srcElement) {
    var param = [];
    param[param.length] = 'WFNo=' + $('#WorkFlowData #WFWFNo').val();
    if ($('#WorkFlowData #WFSigStep').length > 0 &&
        $('#WorkFlowData #WFWFSigSeq').length > 0) {
        param[param.length] = 'SigStep=' + $('#WorkFlowData #WFSigStep').val();
        param[param.length] = 'WFSigSeq=' + $('#WorkFlowData #WFWFSigSeq').val();
    }
    _openWin('workflowRemark', _enumWFAP + '/Pub/Remark', param.join('&'));
    return true;
}

function WorkFlowTerminateFlowButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    _workflowAPIAction = 'TerminateFlow';
    
    _alert('WorkFlowConfirmDialog');
}

function WorkFlowBackToNodeButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    _workflowAPIAction = 'BackToNode';

    _alert('WorkFlowConfirmDialog');
}

function WorkFlowNextToNodeButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    _workflowAPIAction = 'NextToNode';

    _alert('WorkFlowConfirmDialog');
}

function WorkFlowPickNewUserIDButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    _workflowAPIAction = 'PickNewUser';

    _alert('WorkFlowConfirmDialog');
}

function WorkFlowConfirmOKButton_onClick(srcElement) {
    
    if (_workflowAPIAction == 'Signature') {
        _WorkFlowSignature();
    } else if (_workflowAPIAction == 'TerminateFlow') {
        _WorkFlowTerminateFlow();
    } else if(_workflowAPIAction == 'BackToNode') {
        _WorkFlowBackToNode();
    } else if (_workflowAPIAction == 'NextToNode') {
        _WorkFlowNextToNode();
    } else if (_workflowAPIAction == 'PickNewUser') {
        _WorkFlowPickNewUser();
    }

    _alert('WorkFlowAPIResultConfirmDialog', _workflowAPIResult );
}

function WorkFlowConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

function _WorkFlowSignature(srcElement) {
    $.ajax({
        url: '/_BaseAP/GetSignatureResult',
        type: 'POST',
        data: {
            WFNo: $('#WFWFNo').val(),
            NodeNo: $('#WFNodeNo').val(),
            UserID: $('#WFUserID').val(),
            SigResultID: $('input:radio[name=WFSigResultID]:checked').val(),
            SigComment: $('#WFSigComment').val(),
            NewUserID: $('#WFNextNodeNewUserID').val()
        },
        dataType: 'json',
        async: false,
        success: function(response) {
            if (response.Result === 'Y') {
                _workflowAPIResult = _JsMsg_WorkFlowAPIExecuteSuccess;
            } else {
                _workflowAPIResult = response.Message;
            }

            if (typeof (WorkFlow.onSignature) === 'function') {
                WorkFlow.onSignature();
            }
        },
        error: function(jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(_JsMsg_WorkFlowAPISignatureError);
            _ShowJsErrMessageBox();
        }
    });
}

function _WorkFlowTerminateFlow(srcElement) {
    $.ajax({
        url: '/_BaseAP/GetTerminateFlowResult',
        type: 'POST',
        data: { WFNo: $('#WFWFNo').val(), UserID: $('#WFUserID').val() },
        dataType: 'json',
        async: false,
        success: function(response) {
            if (response.Result === 'Y') {
                _workflowAPIResult = _JsMsg_WorkFlowAPIExecuteSuccess;
            } else {
                _workflowAPIResult = response.Message;
            }

            if (typeof (WorkFlow.onTerminateFlow) === 'function') {
                WorkFlow.onTerminateFlow();
            }
        },
        error: function(jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(_JsMsg_WorkFlowAPITerminateFlowError);
            _ShowJsErrMessageBox();
        }
    });
}

function _WorkFlowBackToNode(srcElement) {
    $.ajax({
        url: '/_BaseAP/GetBackToNodeResult',
        type: 'POST',
        data: { WFNo: $('#WFWFNo').val(), UserID: $('#WFUserID').val() },
        dataType: 'json',
        async: false,
        success: function(response) {
            if (response.Result === 'Y') {
                _workflowAPIResult = _JsMsg_WorkFlowAPIExecuteSuccess;
            } else {
                _workflowAPIResult = response.Message;
            }

            if (typeof (WorkFlow.onBackToNode) === 'function') {
                WorkFlow.onBackToNode();
            }
        },
        error: function(jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(_JsMsg_WorkFlowAPIBackToNodeError);
            _ShowJsErrMessageBox();
        }
    });
}

function _WorkFlowNextToNode(srcElement) {
    if (($('#WFIsAssgNextNode').val() == 'Y' && $('#WFNextNodeNewUserID').val() != '') ||
        ($('#WFIsAssgNextNode').val() == 'N')) {
        $.ajax({
            url: '/_BaseAP/GetNextToNodeResult',
            type: 'POST',
            data: { WFNo: $('#WFWFNo').val(), UserID: $('#WFUserID').val(), NewUserID: $('#WFNextNodeNewUserID').val() },
            dataType: 'json',
            async: false,
            success: function(response) {
                if (response.Result === 'Y') {
                    _workflowAPIResult = _JsMsg_WorkFlowAPIExecuteSuccess;
                } else {
                    _workflowAPIResult = response.Message;
                }

                if (typeof (WorkFlow.onNextToNode) === 'function') {
                    WorkFlow.onNextToNode();
                }
            },
            error: function(jqXHR, textStatus, errorThrown) {
                _AddJsErrMessage(_JsMsg_WorkFlowAPINextToNodeError);
                _ShowJsErrMessageBox();
            }
        });
    } else {
        _workflowAPIResult = _JsMsg_WorkFlowNextNodeNewUserError;
    }
}

function _WorkFlowPickNewUser(srcElement) {
    $.ajax({
        url: '/_BaseAP/GetPickNewUserResult',
        type: 'POST',
        data: { WFNo: $('#WFWFNo').val(), UserID: $('#WFUserID').val(), NewUserID: $('#WFUserID').val() },
        dataType: 'json',
        async: false,
        success: function(response) {
            if (response.Result === 'Y') {
                _workflowAPIResult = _JsMsg_WorkFlowAPIExecuteSuccess;
            } else {
                _workflowAPIResult = response.Message;
            }

            if (typeof (WorkFlow.onPickNewUser) === 'function') {
                WorkFlow.onPickNewUser();
            }
        },
        error: function(jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(_JsMsg_WorkFlowAPIPickNewUserError);
            _ShowJsErrMessageBox();
        }
    });
}

function WorkFlowAPIResultConfirmOKButton_onClick(srcElement) {
    if (_workflowAPIResult == _JsMsg_WorkFlowAPIExecuteSuccess) {
        if (_workflowAPIAction == 'PickNewUser') {
            location.reload();
        } else {
            if (window.opener != null) {
                window.close();
            } else {
                location.href = _enumWFAP + '/Pub/ToDoList';
            }
        }
        return true;
    } else {
        _btnUnblockUI(this, false);    
    }
}

function _HideDataBar(srcElement) {
    $('#WorkFlowDataBar').hide();
}

function _HideButtonBar(srcElement) {
    $('#WorkFlowButtonBar').hide();
}

function _HideSignatureButton(srcElement) {
    $('#WorkFlowSignatureButton').hide();
}

function _HideSetSignatureButton(srcElement) {
    $('#WorkFlowSetSignatureButton').hide();
}

function _HideDocumentButton(srcElement) {
    $('#WorkFlowDocumentButton').hide();
}

function _HideRemarkButton(srcElement) {
    $('#WorkFlowRemarkButton').hide();
}

function _HideTerminateFlowButton(srcElement) {
    $('#WorkFlowTerminateFlowButton').hide();
}

function _HideBackToNodeButton(srcElement) {
    $('#WorkFlowBackToNodeButton').hide();
}

function _HideNextToNodeButton(srcElement) {
    $('#WorkFlowNextToNodeButton').hide();
}

function _HideAssginNextNodeNewUserBar() {
    $('#WorkFlowAssginNextNodeNewUserBar').hide();
}

function _HidePickNewUserIDButton(srcElement) {
    $('#WorkFlowPickNewUserIDButton').hide();
}

function _ClickSignatureButton(srcElement) {
    _WorkFlowSignature();
}

function _ClickSetSignatureButton(srcElement) {
    $('#WorkFlowSetSignatureButton').click();
}

function _ClickDocumentButton(srcElement) {
    $('#WorkFlowDocumentButton').click();
}

function _ClickRemarkButton(srcElement) {
    $('#WorkFlowRemarkButton').click();
}

function _ClickTerminateFlowButton(srcElement) {
    _WorkFlowTerminateFlow();
}

function _ClickBackToNodeButton(srcElement) {
    _WorkFlowBackToNode();
}

function _ClickNextToNodeButton(srcElement) {
    _WorkFlowNextToNode();
}

function _ClickPickNewUserIDButton(srcElement) {
    _WorkFlowPickNewUser();
}

function _NoFormAutoCompleteTextBox_GetList(hiddenID, controllerID, actionName, clearText) {
    if (clearText == null) {
        clearText = false;
    }
    var condition;
    var inputValue;
    var isEnNumber = true;
    $('#' + hiddenID + '_text').autocomplete({
        source: function(request, response) {

            //判斷是否有" | "
            inputValue = $('#' + hiddenID + '_text').val();
            if (inputValue.indexOf(" | ") !== -1) {
                inputValue = inputValue.substring(inputValue.indexOf("|") + 1, inputValue.length).trim();
                condition = inputValue;
                $('#' + hiddenID).val(inputValue);
            }

            $.post('/' + controllerID + '/' + actionName, { condition: request.term, workFlowNo: $('#WFWFNo').val() }, function(data) {
                //(inputValue.charCodeAt(i) > 19968 && inputValue.charCodeAt(i) < 40869) 中文
                //48~57 數字 65~90 EN大寫 97~122 EN小寫
                isEnNumber = true; //判斷是否只輸入英數
                if (inputValue != null && inputValue.length !== 0) {
                    for (var i = 0; i < inputValue.length; i++) {
                        if (inputValue.charCodeAt(i) > 122)
                            isEnNumber = false;
                    }
                }

                if (data != null && data.length === 0) {
                    response(null);
                }

                if (data != null) {
                    if (data.length === 1 && isEnNumber) {
                        $('#' + hiddenID + '_text').val(data[0].Text + ' | ' + data[0].Value);
                        $('#' + hiddenID).val(data[0].Value);
                        $('#' + hiddenID + '_autoCompleteState').val('1'); //autoComplete狀態 1為選定

                        var srcElement = $('#' + hiddenID + '_text')[0];
                        eval("if(typeof(" + hiddenID + "_text_onSelected)=='function') { handleing = " + hiddenID + "_text_onSelected(srcElement); }");
                        $('#' + hiddenID).focus();
                        response(null);
                    } else if (data.length >= 1) {
                        response($.map(data, function (item) {
                            return {
                                label: (item.Text + ' | ' + item.Value),
                                value: (item.Value)
                            };
                        }));
                    }
                }
            });
        },
        select: function(event, ui) {
            if (ui.item) {
                $('#' + hiddenID + '_text').val(ui.item.label);
                $('#' + hiddenID).val(ui.item.value);
                $('#' + hiddenID + '_autoCompleteState').val('1'); //autoComplete狀態 1為選定
                ui.item.value = ui.item.label;

                var srcElement = $('#' + hiddenID + '_text')[0];
                eval("if(typeof(" + hiddenID + "_text_onSelected)=='function') { handleing = " + hiddenID + "_text_onSelected(srcElement); }");
            }
        },
        focus: function(event, ui) {
            return false;
        }
    });

    if ($('#' + hiddenID + '_text').val() === '') {
        $('#' + hiddenID).val(inputValue);
        $('#' + hiddenID + '_text').val(inputValue);
    }

    //直接帶出 ID & Name
    function noFormAutoCompleteGetName() {
        inputValue = $('#' + hiddenID + '_text').val();
        $.post('/' + controllerID + '/' + actionName + '?condition=' + encodeURI(inputValue) + '&workFlowNo=' + $('#WFWFNo').val(), function (data) {
            if (data != null) {
                if (data.length === 1 && inputValue !== '') {
                    $('#' + hiddenID + '_text').val(data[0].Text + ' | ' + data[0].Value);
                    $('#' + hiddenID).val(data[0].Value);
                    $('#' + hiddenID + '_autoCompleteState').val('1'); //autoComplete狀態 1為選定

                    eval("if(typeof(" + hiddenID + "_text_onSelected)=='function') { handleing = " + hiddenID + "_text_onSelected(srcElement); }");
                    $('#' + hiddenID).focus();
                    $(".ui-menu-item").parent().hide();
                } else if (data.length > 1) {
                    $('#' + hiddenID).val(inputValue.toUpperCase());
                    $('#' + hiddenID + '_text').val(inputValue.toUpperCase());
                }
            }
        });
    }

    if (clearText === false) {
        noFormAutoCompleteGetName();
    }

    if (clearText === true) {
        $('#' + hiddenID).val('');
        $('#' + hiddenID + '_text').val('');
    }
}

//#endregion

var WorkFlow = {
    onSignature: undefined,
    onTerminateFlow: undefined,
    onBackToNode: undefined,
    onNextToNode: undefined,
    onPickNewUser: undefined
};

//#region Html Helper
var Html = {
    IconClient: function(parameters) {
        var usedocument = (parameters.document == undefined ? document : parameters.document);
        var alink;
        if (parameters.isSignBlank) {
            alink =
                $(usedocument.createElement('a'))
                .attr({
                    'class': 'ico-href',
                    'id': parameters.id,
                    'name': parameters.id,
                    'href': 'javascript:void(0);'
                })
                .html($(usedocument.createElement('span'))
                    .addClass('icon-stack')
                    .append($(usedocument.createElement('i')).addClass('icon-sign-blank').addClass('icon-stack-base'))
                    .append($(usedocument.createElement('i')).addClass('icon-light').addClass(parameters.className)));
        } else {
            alink =
                $(usedocument.createElement('a'))
                .attr({
                    'class': 'ico-href',
                    'id': parameters.id,
                    'name': parameters.id,
                    'href': 'javascript:void(0);'
                })
                .html($(usedocument.createElement('i')).addClass(parameters.className));
        }

        var strKeys = parameters.keys == undefined ? '' : parameters.keys.join(';');
        alink.attr('onclick', 'javascript:_TextsClientLink_onClick($(this),\';' + strKeys + '\');');
        return alink;
    },
    IconSubmit: function(parameters) {
        var usedocument = (parameters.document == undefined ? document : parameters.document);
        var alink;
        if (parameters.isSignBlank) {
            alink =
                $(usedocument.createElement('a'))
                .attr({
                    'class': 'ico-href',
                    'id': parameters.id,
                    'name': parameters.id,
                    'con': parameters.controllerName,
                    'act': parameters.actionName,
                    'href': 'javascript:void(0);'
                })
                .html($(usedocument.createElement('span'))
                    .addClass('icon-stack')
                    .append($(usedocument.createElement('i')).addClass('icon-sign-blank').addClass('icon-stack-base'))
                    .append($(usedocument.createElement('i')).addClass('icon-light').addClass(parameters.className)));
        } else {
            alink =
                $(usedocument.createElement('a'))
                .attr({
                    'class': 'ico-href',
                    'id': parameters.id,
                    'name': parameters.id,
                    'con': parameters.controllerName,
                    'act': parameters.actionName,
                    'href': 'javascript:void(0);'
                })
                .html($(usedocument.createElement('i')).addClass(parameters.className));
        }
        
        var strKeys = parameters.keys == undefined ? '' : parameters.keys.join(';');
        alink.attr('onclick', 'javascript:_TextsSubmitLink_onClick($(this),\';' + strKeys + '\');');
        return alink;
    },
    Tagstyle: function(parameters) {
        var usedocument = (parameters.document == undefined ? document : parameters.document);
        var tag = $(usedocument.createElement('span')).addClass('tagstyle');

        if (parameters.text) {
            tag.append(parameters.text);
        }

        if (parameters.elementList) {
            $(parameters.elementList).each(function(idx, el) {
                tag.append(el);
            });
        }

        if (parameters.isRevmove) {
            tag.append(
                $(usedocument.createElement('a'))
                .attr({
                    'id': parameters.id,
                    'name': parameters.id,
                    'href': 'javascript:void(0);'
                })
                .addClass('ico-href')
                .html($(usedocument.createElement('i')).addClass('icon-remove'))
                .click(function() {
                    var strKeys = parameters.keys == undefined ? '' : parameters.keys.join(';');
                    eval("if(typeof(" + parameters.id + "Remove_onClick)==='function'){ " + parameters.id + "Remove_onClick($(this),strKeys); } else { tag.remove(); }");
                })
            );
        }

        return tag;
    },
    AutoCompleteTextBox: function(parameters) {
        var usedocument = (parameters.document == undefined ? document : parameters.document);
        var name = parameters.name;
        var value = parameters.value;
        var showText = parameters.value;

        var inputTextBox =
            $(usedocument.createElement('input')).attr({
                id: (name + '_text').replace(/\[/g, '_').replace(/\]/g, '_').replace(/\./g, '_'),
                name: name + '_text',
                type: 'text',
                inputtype: 'text',
                onchange: 'javascript:_AutoCompleteTextBox_onChange("' + name + '", this);',
                onblur: 'javascript:_TextBox_onBlur(this);',
                onkeypress: 'javascript:_TextBox_onKeyPress(this);'
            }).val(showText);
            
        if (parameters.className == undefined) {
            inputTextBox.addClass('helper');
            inputTextBox.addClass('input');
        } else {
            inputTextBox.addClass(parameters.className);
        }

        var inputHidden =
            $(usedocument.createElement('input')).attr({
                id: name.replace(/\[/g, '_').replace(/\]/g, '_').replace(/\./g, '_'),
                name: name,
                type: 'hidden',
                inputtype: 'hidden'
            }).val(value);

        var inputHiddenAutoCompleteState =
            $(usedocument.createElement('input')).attr({
                name: name + '_autoCompleteState',
                type: 'hidden',
                inputtype: 'hidden'
            }).val('0');

        return $([inputTextBox[0], inputHidden[0], inputHiddenAutoCompleteState[0]]);
    }
};
//#endregion

//#region jquery 共用
$.fn.ToGroupElementReName = function () {
    $(this).each(function (index, group) {
        $('input,select,textarea,label,div', group).each(function (idxel, el) {
            var IsReplaceID = false;
            var elFor = $(el).attr('for');
            var elId = $(el).attr('id');
            var elName = $(el).attr('name');
            if (elFor) $(el).attr('for', elFor.replace(/\[\d+\]/g, '[' + index + ']'));
            if (elId) {
                $(el).attr('id', elId.replace(/\[\d+\]/g, '[' + index + ']'));
                IsReplaceID = true;
            }
            if (elId && IsReplaceID === false) $(el).attr('id', elId.replace(/_\d+__/g, '_' + index + '__'));
            if (elName) $(el).attr('name', elName.replace(/\[\d+\]/g, '[' + index + ']'));
        });
    });
};
//#endregion

//#region 固定底板
function _RegisterBoxToFixedBottom(srcElement) {
    _BoxToFixedBottom(srcElement);

    $(window).resize(function (event) {
        _BoxToFixedBottom(srcElement);
    });

    $(window).scroll(function (event) {
        if ($(window).scrollTop() + $(window).height() + 50 > $(document).height()) {
            $(srcElement).removeClass('button-fixed-bottom');
        } else {
            $(srcElement).addClass('button-fixed-bottom');
        }
    });
}

function _BoxToFixedBottom(srcElement) {
    $(srcElement).removeClass('button-fixed-bottom');
    var contentHeight = document.body.scrollHeight,
        winHeight = window.innerHeight;
    if ((contentHeight > winHeight) === false) {
        $(srcElement).removeClass('button-fixed-bottom');
    } else {
        $(srcElement).addClass('button-fixed-bottom');
    }
}
//#endregion

//#region 圖表繪製

//圓餅圖
function _BasicPie(containers, pieDataSets) {
    var charts = [],
        $containers = $(containers),
        datasets = pieDataSets;

    $.each(datasets, function (i, dataset) {
        charts.push(new Highcharts.Chart({
            chart: {
                renderTo: $containers[i],
                type: 'pie',
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'top',
                x: 0,
                y: 25
            },
            title: {
                text: ''
            },
            tooltip: {
                formatter: function () {
                    return this.point.name + ': <b>' + _FormatFloat(this.percentage, 2) + ' %</b>';
                }
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: false
                    },
                    showInLegend: true
                }
            },
            series:
                eval(dataset)
        }));
    });
}

//長條圖
function _BasicColumn(renderToName, titleName, listXAxisCateg, xAxisTitleName, yAxisTitleName, listYAxisData, listYAxisName) {
    $(function () {
        var options = {
            chart: {
                renderTo: renderToName,
                defaultSeriesType: 'column'
            },
            title: {
                text: titleName,
                style: {
                    fontWeight: 'bold'
                }
            },
            xAxis: {
                title: {
                    text: xAxisTitleName
                },
                categories: []
            },
            yAxis: {
                title: {
                    text: yAxisTitleName
                }
            },
            series: []
        };
        $.each(listXAxisCateg, function (itemNo, item) {
            options.xAxis.categories.push(item);
        });

        var chart = new Highcharts.Chart(options);
        var series = { data: [] };
        $.each(listYAxisData, function (listno, listvalue) {
            if (typeof (listvalue) != "undefined" && listvalue != "") {
                var listvalueData = listvalue.split(",");
                $.each(listvalueData, function (itemNo, itemvalue) {
                    if (typeof (itemvalue) != "undefined" && itemvalue != "") {
                        series.id = 1;
                        series.name = listYAxisName[listno];
                        series.data.push(parseInt(itemvalue));
                    }
                });
                chart.addSeries(series, false);
                series = { data: [] };
            }
        });
        chart.redraw();
    });
}

//折線圖
function _BasicLine(renderToName, titleName, listXAxisCateg, xAxisTitleName, yAxisTitleName, listYAxisData, listYAxisName) {
    $(function () {
        var options = {
            chart: {
                renderTo: renderToName,
                defaultSeriesType: 'line'
            },
            title: {
                text: titleName,
                style: {
                    fontWeight: 'bold'
                }
            },
            xAxis: {
                title: {
                    text: xAxisTitleName
                },
                categories: []
            },
            yAxis: {
                title: {
                    text: yAxisTitleName
                }
            },
            series: []
        };
        $.each(listXAxisCateg, function (itemNo, item) {
            options.xAxis.categories.push(item);
        });

        var chart = new Highcharts.Chart(options);
        var series = { data: [] };
        $.each(listYAxisData, function (listno, listvalue) {
            if (typeof (listvalue) != "undefined" && listvalue != "") {
                var listvalueData = listvalue.split(",");
                $.each(listvalueData, function (itemNo, itemvalue) {
                    if (typeof (itemvalue) == "undefined" || itemvalue == "") {
                        itemvalue = "0";
                    }
                    series.id = itemNo + 1;
                    series.name = listYAxisName[listno];
                    series.data.push(parseInt(itemvalue));
                });
                chart.addSeries(series, false);
                series = { data: [] };
            }
        });
        chart.redraw();
    });
}

//趨勢圖
function _TrendChart(renderToName, titleName, viewDataDate, xAxisTitle, yAxisTitle, viewItemUnit, decimals, listActualData, listActualName, listCompareData, listCompareName) {
    $(function () {
        var options = {
            chart: {
                renderTo: renderToName,
                Type: 'line',
                plotBorderWidth: 1
            },
            title: {
                text: titleName,
                style: {
                    fontWeight: 'bold'
                }
            },
            xAxis: {
                title: {
                    text: xAxisTitle
                },
                max: viewDataDate-1
            },
            yAxis: {
                title: {
                    text: yAxisTitle
                }
            },
            tooltip: {
                valueSuffix: viewItemUnit,
                valueDecimals: decimals
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'top',
                x: 5,
                y: 0
            },
            plotOptions: {
                line: {            
                    marker: { 
                        enabled: false 
                    }
                } 
            },
            series: []
        };

        var chart = new Highcharts.Chart(options);

        series = { name: [], data: []};

        var randomcolor = ['#C42525','#7cb5ec','#A6C96A','#F28F43', '#8085e9', '#f15c80', '#434348', '#90ed7d', '#e4d354', '#8085e8', '#8d4653', '#91e8e1'];
        
        $.each(listActualData, function(listno, listvalue) {
            if (typeof (listvalue) != "undefined" && listvalue != "") {
                series.name.push(listActualName[listno]);
                series.lineWidth = 5;
                series.color = randomcolor[listno];
                var actualData = listvalue.split(",");
                $.each(actualData, function(itemNo, item) {
                    series.data.push(parseInt(item));
                });
            }
            chart.addSeries(series, false);
            series = { name: [], data: []};
        });
        
        $.each(listCompareData, function (listno, listvalue) {
            if (typeof(listvalue) != "undefined" && listvalue != "") {
                series.name.push(listCompareName[listno]);
                series.color = randomcolor[listno];
                var compareData = listvalue.split(",");
                $.each(compareData, function(itemNo, item) {
                    series.data.push(parseInt(item));
                });
            }
            chart.addSeries(series, false);
            series = { name: [], data: []};
        });
        
        chart.redraw();
    });
}

//#endregion


//region Angularjs
var _NgPatternID = 'ID';
var _NgPatternAlphanumeric = 'Alphanumeric';
var _NgPatternNumber ='Number';
var _NgPatternAlphabet ='Alphabet';
var _NgPatternChinese ='Chinese';
var _NgPatternPhone ='Phone';
var _NgPatternYearMonthDate = 'YearMonthDate';
var _NgPatternYearMonth = 'YearMonth';
var _NgPatternNotChinese = 'NotChinese';
var _NgPatternDatePicker = 'DatePicker';
var _NgPatternDecimal = 'Decimal';
var _NgPatternInteger = 'Integer';

var _TypeChangeEmail ='Email';
var _TypeChangeDate ='Date';
var _TypeChangeWeek ='Week';
var _TypeChangeTime ='Time';
var _TypeChangeUrl ='Url';

var _FilterCurrency ='currency';
var _FilterCurrencyUSD = 'currency:"USD$"';
var _FilterCurrencyJP = 'currency:"JPY￥"';
var _FilterCurrencyNTD = 'currency:"NTD$"';
var _FilterCurrencyRMB = 'currency:"RMB￥"';
var _FilterCurrencyTHB = 'currency:"THB฿"';

// ng-app, AngularJS在DOM的HTMLTag的範圍才能作用。
function _Angular_ngApp(id, ng_app)
{
    if(ng_app != null || ng_app != undefined) 
    {
        $('#' + id).attr("ng-app", ng_app);
    } else {
        $('#' + id).attr("ng-app", id);
    }
}
//如要多個ng-app,請在controller下方宣告_Angular_Multiple_ngApp此段 //使用此指令手動啟動angular的程序應用
function _Angular_Multiple_ngApp(id, ng_app)
{
    angular.boostrap(document.getElementById(id), [ng_app]);
}
// ng-controller, 操作function裡的物件、商業邏輯 //MainCtrl.$inject = ['$scope', '$http'];
function _Angular_ngController(id, ng_controller)
{
    if(ng_controller != null || ng_controller != undefined) 
    {
        $('#' + id).attr("ng-controller", ng_controller + ' as ' + ng_controller + '_AnJS');
    } else {
        $('#' + id).attr("ng-controller",id + ' as ' + id + '_AnJS');
    }
}
// ng-model, ng-model與view作資料雙向繫結 
function _Angular_ngModel(id, ng_model)
{
    if(ng_model != null || ng_model != undefined) 
    {
        $('#' + id).attr("ng-model", ng_model);
    } else {
        $('#' + id).attr("ng-model", id);
    }
}
//轉換input textbox 的型態
function _Angular_TypeChange(id, type)
{
    switch (type)
    {
        case 'Email' :
            $('#' + id).prop('type','email');
            break;
        case 'Date' :
            $('#' + id).prop('type','date');
            break;
        case 'Week' :
            $('#' + id).prop('type','week');
            break;
        case 'Time' :
            $('#' + id).prop('type','time');
            break;
        case 'Url' :
            $('#' + id).prop('type','url');
            break;
    }   
}
//ng-bind與ngModel進行同步更新綁定資料
function _Angular_ngBind(id, ng_bind, filter, bind_array)
{
    if(ng_bind == undefined || filter == undefined || bind_array == undefined) 
    {
        $('#' + id).attr("ng-bind", '');
    }

    if(filter != null  || filter != undefined ) 
    {
        $('#' + id).attr("ng-bind", ng_bind + " | "+ filter);
    } else {
        if(bind_array != null || bind_array != undefined)
        {
            _Angular_ngBindTemplate(id, bind_array);
        } else {
            $('#' + id).attr("ng-bind", ng_bind);
        }
    }
}
//ng-bind-template可綁定多個ng-bind
function _Angular_ngBindTemplate(id, bind_array)
{
    var bind = '';
    for(var value in bind_array) 
    {
        bind += "{{" + bind_array[value] + "}} ";
    }

    $('#' + id).attr("ng-bind-template", bind );
}
//ng-blur, 觸發點擊事件
function _Angular_ngblur(id, ng_blur)
{
    if(ng_blur != null || ng_blur != undefined) 
    {
        $('#' + id).attr("ng-blur", ng_blur + "_onBlurAnJS()");
    } else {
        $('#' + id).attr("ng-blur", '');
    }
}
//ng-click, 觸發點擊事件
function _Angular_ngClick(id, ng_click)
{
    if(ng_click != undefined || ng_click != null) 
    {
        $('#' + id).attr("ng-click", ng_click + "_onClickAnJS()");
    } else {
        $('#' + id).attr("ng-click", id + "_onClickAnJS()");
    }
}
//ng-Dblclick執行滑鼠雙擊事件的功能
function _Angular_ngDblClick(id, ng_Dblclick)
{
    if(ng_Dblclick != undefined || ng_Dblclick != null) 
    {
        $('#' + id).attr("ng-dblclick", ng_Dblclick + "_onDblclickAnJS()");
    } else {
        $('#' + id).attr("ng-dblclick", id + "_onDblclickAnJS()");
    }
}
//ng-submit, 觸發點擊事件
function _Angular_ngSubmit(id, ng_submit)
{
    if(ng_submit != undefined || ng_submit != null) 
    {
        $('#' + id).attr("ng-submit", ng_submit + "_onSubmitAnJS()");
    } else {
        $('#' + id).attr("ng-submit", id + "_onSubmitAnJS()");
    }
}
//ng-readonly控制可輸入元素的唯讀狀態
function _Angular_ngReadonly(id, ng_readonly)
{
    $('#' + id).attr("ng-readonly", ng_readonly);
}
//ng-disabled 控制表單元素的啟用/禁用狀態
function _Angular_ngDisabled(id, ng_disabled)
{
    if(ng_disabled != null || ng_disabled != undefined) 
    {
        $('#' + id).attr("ng-disabled", ng_disabled);
    } else {
        $('#' + id).attr("ng-disabled", '');
    }
}
//ng-show 顯示
function _Angular_ngShow(id, ng_show)
{
    $('#' + id).attr("ng-show", ng_show);
}
//ng-hide 隱藏
function _Angular_ngHide(id,  ng_hide)
{
    $('#' + id).attr("ng-hide", ng_hide);
}
//ng-checked 勾選
function _Angular_ngChecked(id, ng_Checked)
{
    $('#' + id).attr("ng-checked", ng_Checked);
}
//ng-Message 1.3版本提供的驗證功能 簡化驗證程式碼
function _Angular_ngMessages_Paraent(id, ng_Message)
{
    if(ng_Message != null || ng_Message != undefined) 
    {
        $('#' + id).attr("ng-messages", ng_Message);
    } else {
        $('#' + id).attr("ng-messages", '');
    }
}
function _Angular_ngMessage_Child(id, ng_Message)
{
    if(ng_Message != null || ng_Message != undefined) 
    {
        $('#' + id).attr("ng-message", ng_Message);
    } else {
        $('#' + id).attr("ng-message", '');
    }
}
//ng-Class 允許元素變更或加入多種class,或是以動態方式變動元素樣式
function _Angular_ngClass(id, ng_Class)
{
    $('#' + id).attr("ng-class", ng_Class);
}
//ng-Mousedown 滑鼠點擊時觸發事件
function _Angular_ngMousedown(id, ng_Mousedown)
{
    if(ng_Mousedown != null || ng_Mousedown != undefined)
    {
        $('#' + id).attr("ng-mousedown", ng_Mousedown + "_onMousedownAnJS()");
    } else {
        $('#' + id).attr("ng-mousedown", id + "_onMousedownAnJS()");
    }
}
//ng-Mousover 滑鼠近來時觸發事件
function _Angular_ngMousover(id, ng_Mousover)
{
    if(ng_Mousover != null || ng_Mousover != undefined)
    {
        $('#' + id).attr("ng-mousover", ng_Mousover + "_onMouseoverAnJS()");
    } else {
        $('#' + id).attr("ng-mousover", id + "_onMouseoverAnJS()");
    }
}
//ng-Mousleave 滑鼠離開時觸發事件
function _Angular_ngMouseleave(id, ng_Mouseleave)
{
    if(ng_Mouseleave != null || ng_Mouseleave != undefined)
    {
        $('#' + id).attr("ng-mouseleave", ng_Mouseleave + "_onMouseleaveAnJS()");
    } else {
        $('#' + id).attr("ng-mouseleave", id + "_onMouseleaveAnJS()");
    }
}
//ng-keypress 執行鍵盤放開事件的功能(不包含注音輸入)
function _Angular_ngKeypress(id, ng_Keypress)
{
    $('#' + id).attr("ng-keypress", ng_Keypress + "_onkeyPressAnJS()")
}
//ng-maxlength Textbox限制最多字數
function _Angular_ngMaxlength(id, ng_maxlength)
{
    $('#' + id).attr("ng-maxlength", ng_maxlength);
}
//ng-minLength Textbox限制最少字數
function _Angular_ngMinlength(id, ng_minlength)
{
    $('#' + id).attr("ng-minLength", ng_minlength);
}
//ng-List透過”,”分隔記號將input自動轉變為陣列, 支援正規表示式
function _Angular_ngList(id, ng_list)
{
    $('#' + id).attr("ng-list", ng_list);
}
//ngChange :當內容值變更時,啟動此事件
function _Angular_ngChange(id, ng_change)
{
    if(ng_change != null || ng_change != undefined) 
    {
        $('#' + id).attr("ng-change", ng_change + "_onChangeAnJS()");
    } else {
        $('#' + id).attr("ng-change", id + "_onChangeAnJS()");
    }
}
//ngCopy :當滑鼠使用Ctrl+C或是選單複製時執行動作
function _Angular_ngCopy(id, ng_copy)
{
     if(ng_copy != null || ng_copy != undefined)
     {
        $('#' + id).attr("ng-copy", ng_copy + "_onCopyAnJS()");
     } else {
        $('#' + id).attr("ng-copy", id + "_onCopyAnJS()");
     }
}
//ngPaste :當滑鼠執行Ctrl+V或是選單貼上時執行動作
function _Angular_ngPaste(id, ng_Paste)
{
    if(ng_Paste != null || ng_Paste != undefined)
    {
        $('#' + id).attr("ng-paste", ng_Paste + "_onPasteAnJS()");
    } else {
        $('#' + id).attr("ng-paste", id + "_onPasteAnJS()");
    }
}
//ngCut :當滑鼠使用Ctrl+X或是選單剪下時執行動作
function _Angular_ngCut(id, ng_Cut)
{
    if(ng_Cut != null || ng_Cut != undefined)
    {
        $('#' + id).attr("ng-cut", ng_Cut + "_onCutAnJS()");
    } else { 
        $('#' + id).attr("ng-cut", id + "_onCutAnJS()");
    }
}
//ng-model-Options 為觸發事件提供blur及輸入完後延遲1秒
function _Angular_ngModelOptions(id, type)
{
    switch (type)
    {
        case 'blur' :
            $('#' + id).attr("ng-model-options","{ updateOn: 'blur' }");//updateOn: 'blur' 輸入完後才寫入資料
            break;
        case 'delay' :
            $('#' + id).attr("ng-model-options","{ debounce: 1000 }");//debounce: 1000 延遲1秒輸入
            break;
   } 
}
//ng-repeat 迴圈將陣列資料輸出
function _Angular_ngRepeat(id, keyvalue, object, filter, trackby)
{
    if(filter != null || filter != undefined)
    {
        $('#' + id).attr("ng-repeat", keyvalue + " in " + object +" | filter: "+ filter +" track by " + trackby);
    }else{
        $('#' + id).attr("ng-repeat", keyvalue + " in " + object +" track by " + trackby);
    }
}
//ng-options combobox迴圈將陣列資料輸出
function _Angular_ngOptions(id, label, keyvalue, object, filter, trackby)
{
    if(label != undefined && keyvalue != undefined && object != undefined && trackby != undefined) 
    {
        if(filter != null || filter != undefined)
        {
            $('#' + id).attr("ng-options",  label + " for " + keyvalue + " in " + object + " | filter:" + filter +" track by " + trackby);
        } else {
            $('#' + id).attr("ng-options",  label + " for " + keyvalue + " in " + object + " track by "+ trackby);
        }
    } else {
        $('#' + id).attr("ng-options","");
    }
}

function _Angular_OptionsGroupBy(id, label, groupbyby, keyvalue, object, filter, trackby)
{
    if(filter != null || filter != undefined){
        $('#' + id).attr("ng-options",  label + " group by " + groupbyby +" for " + keyvalue + " in " + object + " | filter:" + filter +" track by " + trackby);
    } else {
        $('#' + id).attr("ng-options",  label + " group by " + groupbyby +" for " + keyvalue + " in " + object + " track by "+ trackby);
    }
}










function _Angular_AutoValidLabel(formName, textName, valid, idValid1, idValid2)
{
    $('label#' + idValid1).attr("ng-show", formName +"."+ textName +".$error." +  valid);
    $('label#' + idValid2).attr("ng-show", formName +"."+ textName +".$error.required");
}
function _Angular_TextBox_Email(id, ngModel, ngBlur)
{
    $('#' + id).prop('type','email');
    $('#' + id).attr("ng-model", ngModel);
    $('#' + id).attr("required", true);
    $('#' + id).attr("ng-maxlength", 20);
    $('#' + id).attr("ng-blur", ngBlur +"_onblur()");
}

function _Angular_TextBox_ID(id, ngModel, ngBlur)
{
    $('#' + id).attr("ng-model", ngModel );
    $('#' + id).attr("required", true);
    $('#' + id).attr("ng-maxlength", 10);
    $('#' + id).attr("ng-Pattern", "/[a-zA-Z][0-9]{9}/");
    $('#' + id).attr("ng-blur", ngBlur +"_onblur()");
}

function _Angular_TextBox(id, ngModel, ngBlur)
{
    $('#' + id).attr("ng-model", ngModel);
    $('#' + id).attr("required", true);
    $('#' + id).attr("ng-maxlength", 20);
    $('#' + id).attr("ng-blur", ngBlur +"_onblur()");
}

function _Angular_TextBox_Password_Valid(id, formName) 
{
    $('#' + id).attr("ng-model", id);
    $('#' + id).attr("required", true);
    $('#' + id).attr("ng-maxlength", 20);
    $('#' + id).attr("ng-blur", id +"_onblur()");
    $('#' + id).attr("ng-pattern", "/^[a-z0-9]+$/");

    $('label#' + id).attr("ng-show", formName +"."+ id +".$error.Pattern" );
    $('label#' + id).attr("ng-show", formName +"."+ id +".$error.required");
}

function _Angular_TextBox_Email_Valid(id, formName)
{
    $('#' + id).prop('type','email');
    $('#' + id).attr("ng-model", id);
    $('#' + id).attr("required", true);
    $('#' + id).attr("ng-maxlength", 20);
    $('#' + id).attr("ng-blur", id +"_onblur()");

    $('label#' + id).attr("ng-show", formName +"."+ id +".$error.email" );
    $('label#' + id).attr("ng-show", formName +"."+ id +".$error.required");
}

function _Angular_TextBox_ID_Valid(id, formName)
{
    $('#' + id).attr("ng-model", id);
    $('#' + id).attr("required", true);
    $('#' + id).attr("ng-maxlength", 10);
    $('#' + id).attr("ng-blur", id +"_onblur()");
    $('#' + id).attr("ng-pattern", "/^[A-Z]{1}[0-9]{9}$/");

    $('label#' + id).attr("ng-show", formName +"."+ id +".$error.pattern");
    $('label#' + id).attr("ng-show", formName +"."+ id +".$error.required");
} 

function _Angular_TextBox_Phone_Valid(id, formName)
{
    $('#' + id).attr("ng-model", id);
    $('#' + id).attr("required", true);
    $('#' + id).attr("ng-maxlength", 10);
    $('#' + id).attr("ng-blur", id +"_onblur()");
    $('#' + id).attr("ng-pattern", "/^09[0-9]{8}$/");

    $('label#' + id).attr("ng-show", formName +"."+ id +".$error.pattern" );
    $('label#' + id).attr("ng-show", formName +"."+ id +".$error.required");
}










//Html.InputTextBox()
function _Angular_TextBox_Email(id)
{
    $('#' + id).prop('type','email');
    $('#' + id).attr("ng-model", id);
    $('#' + id).attr("required", true);
    $('#' + id).attr("ng-maxlength", 20);
    $('#' + id).attr("ng-blur", id +"_onblur()");
}

function _Angular_TextBox_ID(id)
{
    $('#' + id).attr("ng-model", id );
    $('#' + id).attr("required", true);
    $('#' + id).attr("ng-maxlength", 10);
    $('#' + id).attr("ng-Pattern", "/^[A-Z]{1}[0-9]{9}$/");
    $('#' + id).attr("ng-blur", id +"_onblur()");
}

function _Angular_TextBox(id)
{
    $('#' + id).attr("ng-model", id);
    $('#' + id).attr("required", true);
    $('#' + id).attr("ng-maxlength", 20);
    $('#' + id).attr("ng-blur", id +"_onblur()");
}

//Html.FormLabel()
function _Angular_FormLabelValid_Message(id, valid)
{
    $('#' + id).attr("ng-message", valid );
}

function _Angular_FormLabelValid_Show(id, valid)
{
    $('#' + id).attr("ng-show", valid);
}

function _Angular_FormLabelFilter(id, filter)
{
    if(filter != null){
        $('#' + id).attr("ng-bind", id + " | " + filter);
    }else{
        $('#' + id).attr("ng-bind", id);
    }
}

//Html.ButtonClient() Html.ButtonSubmit() 
function _Angular_ButtonClient(id, Valid)
{
    $('#' + id).attr("ng-click", id +"_onClick()");

    if(Valid != null || Valid != undefined)
    { 
        $('#' + id).attr("ng-disabled", Valid);
    }
}

function _Angular_ButtonSubmit(id, Valid)
{
    $('#' + id).attr("ng-submit", id +"_onSubmit()");

    if(Valid != null || Valid != undefined)
    {   
        $('#' + id).attr("ng-disabled", Valid);
    }
}

//Html.InputComboBoxFor()
function _Angular_ComboBox(id, label, keyvalue, object, trackby, filter)
{
    $('#' + id).attr("ng-model", id );

    if(filter != null || filter != undefined)
    {
        $('#' + id).attr("ng-options", label + " for " + keyvalue + " in " + object + " | filter:" + filter +" track by " + trackby );
    } else {
        $('#' + id).attr("ng-options", label + " for " + keyvalue + " in " + object + " track by "+ trackby);
    }
}

//Html.InputCheckBox()
function _Angular_CheckBox(id)
{
    $('#' + id).attr("ng-model", id);
    $('#' + id).attr("ng-checked", id);
    $('#' + id).attr("ng-click", id +"_onClick()");
    $('#' + id).attr("ng-required", true);
}

//Html.InputRadioButton
function _Angular_Radio(id)
{
    $('#' + id).attr("ng-model", id);
}










var ctrlElement;

function _Angular_AutoAddPara(formElement)
{
    var formId = formElement.id;
    var result = true;
    var arrayText = [];
    var arrayCheck = [];
    var arrayRadio = [];
    var arrayComboBox = [];
    
    _Angular_Form(formId);
    
    angular
      .module(formId , ['ngMessages','shareAJAX','Valid'])
      .controller(formId , AngularJs_Contr);

    function AngularJs_Contr($scope, $http, $q, ajax)
    {
        ctrlElement = this;

        if(arrayText!= null || arrayText != undefined) 
        {
            for(var value in arrayText) 
            {
                if(arrayText[value] != "") 
                {
                    eval("ctrlElement."+ arrayText[value] +" = $('#"+ arrayText[value] +"').val();");
                    eval("if(typeof(" + arrayText[value] + "_onblurAnJS)==='function'){ ctrlElement." + arrayText[value] + "_onblurAnJS = "+ arrayText[value] +"_onblurAnJS}");
                }
            }
        }
        if(arrayCheck != null || arrayCheck != undefined) 
        {
            for(var value in arrayCheck) 
            {
                if(arrayCheck[value] != "") 
                {
                    eval("if($('input[name="+ arrayCheck[value] +"]:checked').val() == 'Y') { ctrlElement."+ arrayCheck[value] +" = true; } else { ctrlElement."+ arrayCheck[value] +" = false; }");
                }
            }
        }
        if(arrayRadio != null || arrayRadio != undefined) 
        {
            for(var value in arrayRadio) 
            {
                if(arrayRadio[value] != "") 
                {
                    eval("ctrlElement."+ arrayRadio[value] +" = $('input[name="+ arrayRadio[value] +"]:checked').val();");
                }
            }
        }
        if(arrayComboBox != null || arrayComboBox != undefined) 
        {
            var valueComboBox;
            for(var value in arrayComboBox) 
            {
                if(arrayComboBox[value] != "")
                {
                    valueComboBox = $('#'+ arrayComboBox[value]).attr('originalvalue');
                    if(valueComboBox !="") {
                        eval("ctrlElement."+ arrayComboBox[value] +" = {ItemID: valueComboBox }");
                    } else {
                        eval("ctrlElement."+ arrayComboBox[value] +" = {ItemID: null }");
                    }
                    eval("if(typeof(" + arrayComboBox[value] + "_onChangeAnJS)==='function'){ ctrlElement." + arrayComboBox[value] + "_onChangeAnJS = "+ arrayComboBox[value] +"_onChangeAnJS}");
                }
            }
        }
        if(result == true) 
        {
            eval("if(typeof(" + formId + "_AnJS)==='function'){ result = " + formId + "_AnJS($scope, ctrlElement, ajax, $q); }");
        }
    }

    if($('#' + formId + ' input[type="text"]') != null || $('#' + formId + ' input[type="text"]') != undefined || $('#' + formId + ' input[type="button"]') != null || $('#' + formId + ' input[type="button"]') != undefined) 
    {
        $('#' + formId + ' input[type]').each(function () 
        {
            if ((this.type == 'text' || this.type == 'password') && this.id != 'PageSize' && this.name != 'PageSize') 
            {
                _Angular_AutoAddTextBox(formElement, this);
                arrayText.push(this.id);
            }
            if(this.type == 'button') 
            {
                _Angular_ngClick(this.id, formId + '_AnJS.' + this.id);
            }
        });
    }

    if($('#' + formId + ' input[type="checkbox"]') != null || $('#' + formId + ' input[type="checkbox"]') != undefined)
    {
        $('#' + formId + ' input[type="checkbox"]').each(function() 
        { 
            if($('#' + formId +' input[type="checkbox"]input[name="' + this.name + '"]').length > 1) 
            {
                $('#' + formId +' input[type="checkbox"]input[name="' + this.name + '"]').attr('ng-checked', formId + '_AnJS.' + this.id);
                return false;
            } else {
                _Angular_ngModel(this.id, formId + '_AnJS.' + this.id);
                arrayCheck.push(this.id);
                return true;
            }
        });
    }

    if($('#' + formId + ' input[type="radio"]') != null || $('#' + formId + ' input[type="radio"]') != undefined) 
    {
        $('#' + formId + ' input[type="radio"]').each(function() 
        {
            $('#' + formId + ' input[type="radio"]').attr('ng-model', formId + '_AnJS.' + this.id);
            arrayRadio.push(this.id);
            return false;
        });
    }

    if($('#' + formId +' select') != null || $('#' + formId +' select') != undefined) 
    {
        $('#' + formId +' select').each(function() 
        {
            if(this.id != 'PageIndex' && this.name != 'PageIndex') 
            {
                if($('#' + this.id).attr('ng-options') != null || $('#' + this.id).attr('ng-options') != undefined) 
                {
                    _Angular_ngModel(this.id, formId + '_AnJS.' + this.id);
                    _Angular_ngChange(this.id, formId + '_AnJS.' + this.id);
                    if($(this).attr('isrequired') != null || $(this).attr('isrequired') != undefined) 
                    {
                        _Angular_ngMatchRequire(this.id);
                    }
                    _Angular_Validation(formElement, this);
                    arrayComboBox.push(this.id);
                }
            }
        });
    }
    return result;
}
function _Angular_Form(id)
{
    $('#' + id).attr("name", id);
    $('#' + id).attr("ng-app", id);
    $('#' + id).attr("ng-controller",id + ' as ' + id + '_AnJS');
    $('#' + id).attr("novalidate",'');
}

function _Angular_AutoAddTextBox(formElement, srcElement)
{
     var IsValidation = $(srcElement).attr("validation");
     var maxLength = srcElement.maxLength;
     var minLength = $(srcElement).attr('minimumlength');
     var id = srcElement.id;
     var formId = formElement.id;

     $('#' + id).attr("required", true);
     _Angular_ngModel(id, formId + '_AnJS.' + id);
     _Angular_ngMatchMaxlength(id, maxLength);
     if(minLength > 0) 
     {
         _Angular_ngMinlength(id, minLength);
     }
     _Angular_ngblur(id, formId + '_AnJS.' + id);
     _Angular_ngKeypress(id, formId + '_AnJS.' + id);
     _Angular_ngPattern(id);

     _Angular_CheckType(srcElement)
    
     if(IsValidation == "") 
     {
         _Angular_Validation(formElement, srcElement);
     }
}
function _Angular_Validation(formElement, srcElement)
{         
    var wResources = GetInputTypeResources($(srcElement).attr('inputtype'));
    var isNgPattern = $(srcElement).attr('ng-pattern');
    var isRequired = $(srcElement).attr('isrequired');
    var isMatchRequire = $(srcElement).attr('ng-Match-Require');
    var isMinlength = $(srcElement).attr('minimumlength');
    var isMaxlength = $(srcElement).attr('maxlength');
    var titleName = $(srcElement).attr('titlename');
    var id = srcElement.id;
    var textName = srcElement.name;
    var formName = formElement.name;

    if(isNgPattern != null || isNgPattern != undefined) 
    {
        $('#' + id).after('<span style="color:red" ng-show="' + formName + '.' + textName + '.$error.pattern">' + wResources.InputTypeResource + '</span>');
    }
    if(isMinlength > 0) 
    {
        $('#' + id).after('<span style="color:red" ng-show="' + formName + '.' + textName + '.$error.minlength">' + _Least + isMinlength + wResources.InputTypeCodeResource + '</span>');
    }
    if(isMaxlength != null || isMaxlength != undefined) 
    {
        $('#' + id).after('<span style="color:red" ng-show="' + formName + '.' + textName + '.$error.MatchMaxlength">' + _Most + isMaxlength + wResources.InputTypeCodeResource + '</span>');
    }
    if(isRequired != null || isRequired != undefined) 
    {
        if($('select#'+ id)[0] != null || $('select#'+ id)[0] != undefined) 
        {
            $('#' + id).after('<span style="color:red" ng-show="' + formName + '.' + textName + '.$error.MatchRequire && ' + formName + '.' + textName + '.$dirty">' + titleName + ' ' + wResources.Required + '</span>');
        } else {
            $('#' + id).after('<span style="color:red" ng-show="' + formName + '.' + textName + '.$error.required && ' + formName + '.' + textName + '.$dirty">' + wResources.Required + '</span>');
        }
    }
}
function _Angular_CheckType(srcElement)
{
    if($(srcElement).attr('inputtype') != null || $(srcElement).attr('inputtype') != undefined) 
    {
             switch($(srcElement).attr('inputtype')) 
             {
                case 'TextBoxNotChinese' :
                     _Angular_ngPattern(srcElement.id, _NgPatternNotChinese);
                     break;
                case 'TextBoxAlphanumeric' :
                     _Angular_ngPattern(srcElement.id, _NgPatternAlphanumeric);
                     break;
                case 'TextBoxNumber' :
                     _Angular_ngPattern(srcElement.id, _NgPatternNumber);
                     break;
                case 'TextBoxInteger' :
                     _Angular_ngPattern(srcElement.id, _NgPatternInteger);
                     break;
                case 'TextBoxDecimal' :
                     _Angular_ngPattern(srcElement.id, _NgPatternDecimal);
                     break;
                case 'TextBoxIdNo': 
                     _Angular_ngPattern(srcElement.id, _NgPatternID);
                     break;
                case 'TextBoxChar8':
                     _Angular_ngPattern(srcElement.id, _NgPatternYearMonthDate);
                     break;
                case 'TextBoxInterval':
                     _Angular_ngPattern(srcElement.id, _NgPatternYearMonthDate);
                     break;
                case 'TextBoxYearMonth':
                     _Angular_ngPattern(srcElement.id, _NgPatternYearMonth);
                     break;
                case 'TextBoxDatePicker' :
                     _Angular_ngPattern(srcElement.id, _NgPatternDatePicker);
                     break;
             }
      }
}

function _Angular_ngPattern(id, type)
{
    //ng-Pattern 正規表示式 例如:ng-pattern="/[a-zA-Z][0-9]{9}/“  限制 RegExp 格式
    if(type != null || type != undefined) 
    {
        switch(type)
        {
            case 'ID':
                $('#' + id).attr("ng-pattern", "/^[A-Z]{1}[0-9]{9}$/");
                break;
            case 'Alphanumeric':
                $('#' + id).attr("ng-pattern", "/^[a-zA-Z0-9]+$/");
                break;
            case 'Alphabet':
                $('#' + id).attr("ng-pattern", "/^[a-zA-Z]+$/");
                break;
            case 'Number':
                $('#' + id).attr("ng-pattern", "/^[0-9]+$/");
                break; 
            case 'Decimal':
                $('#' + id).attr("ng-pattern", "/^(\-)?\d+(\.?)+?[0-9]*$/");
                break;
            case 'Integer':
                $('#' + id).attr("ng-pattern", '/^(\-)?[0-9]*$/');
                break;
            case 'NotChinese':
                $('#' + id).attr("ng-pattern", "/^[\u4e00-\u9fa5]+$/");
                break;
            case 'Phone': 
                $('#' + id).attr("ng-pattern", "/^09[0-9]{8}$/");
                break;
            case 'YearMonth' :
                $('#' + id).attr("ng-pattern", "/^((19|20)[0-9]{2}((0)[1-9]|(1)[012]))*$/");
                break;
            case 'YearMonthDate' :
                $('#' + id).attr("ng-pattern", "/^((19|20)[0-9]{2}((0)[1-9]|(1)[012])(0[1-9]|[12][0-9]|3[01]))*$/");
                break;
            case 'DatePicker':
                $('#' + id).attr("ng-pattern", "/^((19|20)[0-9]{2}((0)[1-9]|(1)[012])(0[1-9]|[12][0-9]|3[01]))*$/");
                break;
        }
    }
}

//預設下拉選單為空值
function _Angular_ComboBoxLink(id) 
{
    eval("ctrlElement." + id + " = { 'ItemID' : null };")
}
//一次勾選全部Checkbox
function _Angular_CheckBoxed(id_check, id_checked)
{
    $('#'+ id_check).attr('ng-model','checked');
    $('input[type="checkbox"]#'+ id_checked).attr('ng-checked','checked');
}
//一次勾選全部Checkbox
function _Angular_CheckBoxedLike(id_check, id_checked)
{
    $('input[type="checkbox"]input[name^='+ id_checked +']').removeAttr('ng-model');

    $('#'+ id_check).attr('ng-model','checked');
    $('input[type="checkbox"]input[name^='+ id_checked +']').attr('ng-checked','checked');
}
//AngularJS 下拉選單
function _Angular_ComboBoxList(id, formElement, filter) 
{
    var filterPlus = '';
    var form = formElement.id;

    if (filter != null || filter != undefined) 
    {
        if(filter.length > 1) 
        {
            for(var value in filter) 
            {
               filterPlus += ' | filter: ' + form + '_AnJS.' + filter[value] + '.ItemID:true';
            }
            $('#' + id).attr("ng-options", "data.ItemNM for data in " + form + "_AnJS." + id + "DataList "+ filterPlus +"  track by data.ItemID");
        } else {
            $('#' + id).attr("ng-options", "data.ItemNM for data in " + form + "_AnJS." + id + "DataList | filter: " + form + "_AnJS." + filter + ".ItemID:true track by data.ItemID");
        }
    } else {
        $('#' + id).attr("ng-options", "data.ItemNM for data in " + form + "_AnJS." + id + "DataList track by data.ItemID");
    }
}
//AngularJS 下拉選單(群組)
function _Angular_ComboBoxListGroup(id, formElement, filter) 
{
    var filterPlus ='';
    var form = formElement.id;

    if (filter != null || filter != undefined) 
    {
        if(filter.length > 1) 
        {
            for(var value in filter) 
            {
                filterPlus += ' | filter: ' + form + '_AnJS.' + filter[value] + '.ItemID:true';
            }
            $('#' + id).attr("ng-options", " data.ItemNM group by data.ItemParentNM for data in " + form + "_AnJS." + id + "DataList " + filterPlus + " track by data.ItemID");
        } else {
            $('#' + id).attr("ng-options", " data.ItemNM group by data.ItemParentNM for data in " + form + "_AnJS." + id + "DataList | filter:" + form + "_AnJS." + filter + ".ItemID:true track by data.ItemID");
        }
    } else {
        $('#' + id).attr("ng-options", " data.ItemNM group by data.ItemParentNM for data in " + form + "_AnJS." + id + "DataList track by data.ItemID");
    }
}

//自訂Combobox必選
function _Angular_ngMatchRequire(id)
{
    $('#' + id).attr('ng-Match-Require','');
}
//自訂Maxlength長度限制提示
function _Angular_ngMatchMaxlength(id, ng_Match_Maxlength)
{
    if(ng_Match_Maxlength != null || ng_Match_Maxlength != undefined) 
    {
        $('#'+ id).attr('ng-Match-Maxlength', ng_Match_Maxlength);
    } else {
        $('#'+ id).attr('ng-Match-Maxlength','');
    }
}
//自訂Shift與CapsLock大寫注意
function _Angular_ngMatchShift(id, formElement)
{
    var formName = formElement.name;
    $('#' + id).attr('ng-Match-Shift','');
    $('#' + id).after('<span style="color:red" ng-show="'+ formName +'.' + id + '.$error.MatchShift">' + _JsMsg_CapsLockRemind + '</span>');
}

//endregion Angularjs