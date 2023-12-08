/// <reference path="/Scripts/LionTechWebERPHelpers.js" />
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

    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }

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

    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }

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

    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }

    if (Result) {
        eval("if(typeof(" + ElementId + "_onEnter)==='function'){ Result = " + ElementId + "_onEnter(srcElement); }");
    }

    return Result;
}
//#endregion

//#region AutoCompleteTextBox 共用
function _AutoCompleteTextBox_onChange(hiddenID, srcElement) {
    if ((hiddenID.match('[[]') != 'null' || hiddenID.match('[[]') != undefined) &&
        (hiddenID.match('[]]') != 'null' || hiddenID.match('[]]') != undefined) &&
        (hiddenID.match('[.]') != 'null' || hiddenID.match('[.]') != undefined)) {
        hiddenID = hiddenID.replace(".", "_").replace("[", "_").replace("]", "_");
    }
    var inputValue = $('#' + hiddenID + '_text').val().toUpperCase();

    //判斷是否有" | "
    if (inputValue.indexOf(" | ")!=-1) {
        inputValue = inputValue.substring(inputValue.indexOf("|")+1, inputValue.length).trim();
    }

    var form = _GetParentElementByTag(srcElement, "FORM");
    if (form != null ) {
        var formID = form.id;

        if (inputValue.indexOf(" | ")!=-1) {
            $('#' + hiddenID + '_text', $('#' + formID)).val('');
            $('#' + hiddenID, $('#' + formID)).val('');
        }

        if ($('#' + hiddenID + '_text').val() == '') {
            $('#' + hiddenID + '_text', $('#' + formID)).val(inputValue);
            $('#' + hiddenID, $('#' + formID)).val(inputValue);
        }

        $('#' + hiddenID + '_autoCompleteState', $('#' + formID)).val('2');
    } else {

        $('#' + hiddenID + '_text').val('');
        $('#' + hiddenID).val('');

        if ($('#' + hiddenID + '_text').val() == '') {
            $('#' + hiddenID + '_text').val(inputValue);
            $('#' + hiddenID).val(inputValue);
        }

        $('#' + hiddenID + '_autoCompleteState').val('2');
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

    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }

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

    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }

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

    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }

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
    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }
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

    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }

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

    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }

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
    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }
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

    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }

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

    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }

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
    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }

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
    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
        rowName = ID.split(".")[0];
    } else {
        ElementId = ID;
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
    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }

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
    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }

    _ClossJsErrMessageBox();

    if ($("select[id=PageIndex]").length > 0) {
        $("select[id=PageIndex]").val("1");
    }

    if (Result) {
        eval("if(typeof(" + ElementId + "_onClick)==='function'){ Result = " + ElementId + "_onClick(srcElement,keys); }");
    }
    if (Result) {
        _submit($(srcElement));
    };
}
//#endregion

//#region _ImageButton_onClick共用
function _ImageButton_onClick(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";
    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }
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
    var action = "/" + con + "/" + act;

    eval("if(typeof(" + srcElement.id + "_onClick)=='function') { handleing = " + srcElement.id + "_onClick(srcElement,keys); } else { window.location.href = action; }");

    return handleing;
}
//#endregion

//#region BasicCode 共用
function _BasicCode_onBlur(srcElement) {
    var Result = true;
    var ID = $(srcElement).attr('id');
    var ElementId = "";
    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }
    if (Result) {
        eval("if(typeof(" + ElementId + "_onBlur)==='function'){ Result = " + ElementId + "_onBlur(srcElement); }");
    }
    
    if (srcElement.value != srcElement.getAttribute("originalvalue")) {
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

    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }

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

    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
    } else {
        ElementId = ID;
    }

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
    if (ID.split(".").length > 1) {
        ElementId = ID.split(".")[1];
        rowName = ID.split(".")[0];
    } else {
        ElementId = ID;
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
    };

    var features = '';
    $.each(objfeatures, function (key, value) {
        features += key + '=' + value + ',';
    });
    features = features.substring(0, features.length - 1);
    var _newWindow;
    try {
        _newWindow = window.open("about:blank", windowName +　"LionTech_blank", features, true);
        //因加掛document.domain 時 ie另開空白頁時 會發生存取問題所以用location 寫javascript 把document.domain 加掛上去
        _newWindow.location = "javascript:document.write('"
        + "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">\n"
        + "<html><head><script type=\"text/javascript\">document.domain=\"" + document.domain + "\"</script></head><body></body></html>');document.close()";
        _newWindow.status;
    } catch (e) {
        //若跟不同domain的網站用到同名字會無法關閉真的會發生問題的話要在網站上放一個空白頁去開 不用about:blank
        _newWindow.close();
        //因關掉的速度太快 ie無法再另開同名字的視窗所以放慢速度.
        setTimeout(function () {
            _newWindow = window.open("about:blank", windowName + "LionTech_blank", features, true);
            if (_newWindow != null) { //被檔快顯會有問題.
                _newWindow.location = "javascript:document.write('"
                + "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">\n"
                + "<html><head><script type=\"text/javascript\">document.domain=\"" + document.domain + "\"</script></head><body></body></html>');document.close()";
            }
        }, 50);
    }

    setTimeout(function () {
        if (_newWindow != null) {
            _newWindow.document.charset = "UTF-8";
            var html = "";
            html += "<html><head></head><body><form id='formid' method='post' action='" + url + "'>";
            html += "<input type='hidden' name='OpenWin' value='1'>";
            if (parameters != undefined) {
                var strQuery = parameters.split('&');
                for (i = 0; i < strQuery.length; i++) {
                    var strTemp = strQuery[i].split('=');
                    if (strTemp.length == 2) {
                        html += "<input type='hidden' name='" + strTemp[0] + "' value='" + strTemp[1] + "'>";
                    }
                }
            }
            html += "</form><script type='text/javascript'>document.getElementById(\"formid\").submit()</script></body></html>";
            _newWindow.document.write(html);
            return _newWindow;
        } else {
            return false;
        }
    }, 100);

    return _newWindow;
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
        $('#' + srcElement + ' tr', formElement).removeClass('tr-selected');
        $(this).addClass('tr-selected');
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
        changeYear: true
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
                    var integerExpression = /^(\-)?\d+(\.?)+?\d*$/;
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
    var h = /^[a-z](1|2)\d{8}$/i;
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
        var reg = getStringFormatPlaceHolderRegEx(i);
        s = s.replace(reg, (arguments[i + 1] == null ? "" : arguments[i + 1]));
    }
    return cleanStringFormatResult(s);
}

//讓輸入的字串可以包含{}
function getStringFormatPlaceHolderRegEx(placeHolderIndex) {
    return new RegExp('({)?\\{' + placeHolderIndex + '\\}(?!})', 'gm')
}

//當format格式有多餘的position時，就不會將多餘的position輸出
//ex:
// var fullName = 'Hello. My name is {0} {1} {2}.'.format('firstName', 'lastName');
// 輸出的 fullName 為 'firstName lastName', 而不會是 'firstName lastName {2}'
function cleanStringFormatResult(txt) {
    if (txt == null) return "";
    return txt.replace(getStringFormatPlaceHolderRegEx("\\d+"), "");
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
    }
    else {
        tblAsJQueryObject.attr("data-item-original-width", tblAsJQueryObject.width());
        tblAsJQueryObject.find('thead tr th').each(function () {
            $(this).attr("data-item-original-width", $(this).width());
        });
        tblAsJQueryObject.find('tbody tr:eq(0) td').each(function () {
            $(this).attr("data-item-original-width", $(this).width());
        });

        var tblDiv = $("<div id='tbodyDiv'></div>");
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