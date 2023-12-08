;
(function ($) {
    $.widget('ui.queryBuilder', {
        options: {
            modelName: '',
            isReadOnly: false,
            filters: [],
            rules: {}
        },
        _create: function () {
            this.id = this.element.attr('id');
            this._groupConditionsRadioNumber = 0;
            this._groupRules = $(document.createElement('dl')).addClass('rules-group-container');
            this.element.append(this._groupRules);

            this._generateRoleList({
                obj: this.options.rules,
                objParentName: this.options.modelName,
                dl: this._groupRules,
                isGenerateGroupRuleDeleteButton: false
            });
            this._generateStyle();
        },
        _generateStyle: function () {
            var style = document.createElement('style');
            style.type = 'text/css';
            style.innerHTML = '.queryBuilder-placeholder-highlight { border: 2px solid #AAAAAA; font-weight: bold; border-style:dashed; }';
            document.getElementsByTagName('head')[0].appendChild(style);
        },
        _dragEvent: function () {
            var sortableObject = $('dl > dd > ul', $(this.element));
            var self = this;

            sortableObject.sortable(
            {
                connectWith: sortableObject,
                cursor: 'all-scroll',
                forcePlaceholderSize: true,
                placeholder: 'queryBuilder-placeholder-highlight',
                opacity: 0.5,
                handle: '.icon-move',
                distance: 0.1,
                stop: function (event, ui) {
                    self._elementReName();
                }
            });
        },
        _generateRoleList: function (para) {
            var self = this;
            var dd = $(document.createElement('DD'));
            var ul = $(document.createElement('UL')).addClass('rules-list');

            for (var prop in para.obj) {
                if (para.obj.hasOwnProperty(prop)) {
                    var objName = para.objParentName;
                    if (para.obj.hasOwnProperty(prop)) {
                        if (prop === 'RuleList' || prop === 'GroupRuleList' || $.isNumeric(prop)) {
                            if ($.isNumeric(prop)) {
                                self._generateRoleList({
                                    obj: para.obj[prop],
                                    objParentName: objName + '[' + prop + ']',
                                    dl: para.dl,
                                    isGenerateGroupRuleDeleteButton: true
                                });
                            } else {
                                var index;
                                var obj;
                                if (prop === 'RuleList') {
                                    for (index = 0; index < para.obj[prop].length; index++) {
                                        obj = para.obj[prop][index];
                                        ul.append(self._generateRoleContainer({
                                            ID: obj.ID,
                                            Operator: obj.Operator,
                                            Value: obj.Value,
                                            Name: objName + '.' + prop + '[' + index + ']'
                                        }));
                                    }
                                }

                                if (prop === 'GroupRuleList') {
                                    for (index = 0; index < para.obj[prop].length; index++) {
                                        obj = para.obj[prop][index];
                                        var dl = $(document.createElement('DL')).addClass('rules-group-container');
                                        self._generateRoleList({
                                            obj: obj,
                                            objParentName: objName + '.' + prop + '[' + index + ']',
                                            dl: dl,
                                            isGenerateGroupRuleDeleteButton: true
                                        });
                                        ul.append(dl);
                                    }
                                }
                            }
                        } else if (prop === 'Condition') {
                            para.dl.append(self._generateGroupRoleHeader({
                                Value: para.obj[prop],
                                Name: objName,
                                PropName: objName + '.Condition',
                                isGenerateDeleteButton: para.isGenerateGroupRuleDeleteButton
                            }));
                        }
                    }
                }
            }
            
            dd.append(ul);
            para.dl.append(dd);
            if (self.options.isReadOnly === false) {
                $('select[name$="Value"]', this.element).combobox();
            }
            
            $('div#QueryBuilderBox li.rule-container select[name$=Operator]').change();
            
            self._dragEvent();
        },
        _generateGroupRoleHeader: function (para) {
            var self = this;
            var dt = $(document.createElement('DT')).addClass('rules-group-header');
            var buttonBox = $(document.createElement('DIV')).addClass('btn-group').addClass('pull-right').addClass('group-actions');

            if (self.options.isReadOnly === false) {
                var addRuleButton = $(document.createElement('input')).attr({
                    type: 'button',
                    value: $.ui.queryBuilder.locale.AddRoleRuleButton
                }).click(function () {
                    $('>ul', dt.next('dd')).append(
                        self._generateRoleContainer({
                            ID: '',
                            Operator: '',
                            Value: '',
                            Name: para.Name + '.RuleList[1]'
                        }));
                    $('select[name$="Value"]', $(this).closest('dl')).combobox();
                    self._dragEvent();
                    self._elementReName();
                });

                var addGroupButton = $(document.createElement('input')).attr({
                    type: 'button',
                    value: $.ui.queryBuilder.locale.AddGroupRoleButton
                }).click(function () {
                    var dl = $(document.createElement('DL')).addClass('rules-group-container');
                    self._generateRoleList({
                        obj: {
                            Condition: 'AND',
                            RuleList: [
                                {
                                    ID: '',
                                    Operator: '',
                                    Value: ''
                                }
                            ]
                        },
                        objParentName: para.Name + '.GroupRuleList[999999]',
                        dl: dl,
                        isGenerateGroupRuleDeleteButton: true
                    });
                    $('>dd>ul', $(this).closest('dl')).append(dl);
                    $('select[name$="Value"]', $(this).closest('dl')).combobox();
                    self._dragEvent();
                    self._elementReName();
                });
                buttonBox.append(addRuleButton).append(addGroupButton);

                if (para.isGenerateDeleteButton) {
                    var deleteGroupButton = $(document.createElement('input')).attr({
                        type: 'button',
                        value: $.ui.queryBuilder.locale.DeleteGroupRoleButton
                    }).click(function () {
                        $(this).closest('dl').remove();
                        self._elementReName();
                    });
                    buttonBox.append(deleteGroupButton);
                }
            }

            var radioButtonBox = $(document.createElement('DIV'))
                .addClass('btn-group').addClass('group-conditions')
                .prepend(Html.IconClient({ id: 'MoveIcon', className: 'icon-move' }));
            var andRadioButton = $(document.createElement('input')).attr({
                id: para.PropName + '1',
                name: para.PropName,
                type: 'radio',
                value: 'AND'
            });
            var orRadioButton = $(document.createElement('input')).attr({
                id: para.PropName + '2',
                name: para.PropName,
                type: 'radio',
                value: 'OR'
            });

            var andLabel = $(document.createElement('label')).attr('for', para.PropName + '1').html($.ui.queryBuilder.locale.ANDCondition);
            var orLabel = $(document.createElement('label')).attr('for', para.PropName + '2').html($.ui.queryBuilder.locale.ORCondition);

            if (para.Value === 'AND') {
                andRadioButton.attr('checked', 'checked');
            } else {
                orRadioButton.attr('checked', 'checked');
            }
            radioButtonBox.append(andRadioButton).append(andLabel).append(orRadioButton).append(orLabel);

            dt.append(buttonBox);
            dt.append(radioButtonBox);
            self._dragEvent();
            return dt;
        },
        _generateRoleContainer: function (para) {
            var filter;
            var index;
            var self = this;
            var ruleDragBtn = Html.IconClient({ id: 'MoveIcon', className: 'icon-move' });
            var li = $(document.createElement('LI')).addClass('rule-container');
            var roleActionBox = $(document.createElement('DIV')).addClass('btn-group').addClass('pull-right').addClass('rule-actions');
            var filterBox = $(document.createElement('DIV')).addClass('rule-filter-container');
            var operatorBox = $(document.createElement('DIV')).addClass('rule-operator-container');
            var valueBox = $(document.createElement('DIV')).addClass('rule-value-container');
            var filterSelect = $(document.createElement('select')).attr({ name: para.Name + '.ID' });
            var operatorSelect = $(document.createElement('select')).attr({ name: para.Name + '.Operator' });
            var valueInput;

            var generateValueElement = function (parameters, id) {
                valueInput = $(document.createElement('select')).attr({ name: para.Name + '.Value' });

                for (index = 0; index < self.options.filters.length; index++) {
                    filter = self.options.filters[index];
                    if (filter.id === id) {
                        for (var i = 0; i < filter.values.length; i++) {
                            valueInput.append($(document.createElement('option')).attr('value', filter.values[i].Value).html(filter.values[i].Text));
                        }
                    }
                }

                valueInput.val(parameters.Value);
                valueBox.html(valueInput);
                self._elementReName();
            }

            var deleteButton = $(document.createElement('input')).attr({
                type: 'button',
                value: $.ui.queryBuilder.locale.DeleteButton
            }).click(function () {
                li.remove();
                self._elementReName();
            });


            // filter
            filterSelect.append($(document.createElement('option')).attr('value', ''));
            for (index = 0; index < this.options.filters.length; index++) {
                filter = this.options.filters[index];
                filterSelect.append($(document.createElement('option')).attr('value', filter.id).html(filter.label));
            }
            filterSelect
                .val(para.ID)
                .change(function () {
                    generateValueElement(para, this.value);
                    operatorSelect.val('Equal');
                    $('select[name$="Value"]', $(this).closest('li')).combobox();
                });


            // operator
            for (index = 0; index < $.ui.queryBuilder.operators.length; index++) {
                filter = $.ui.queryBuilder.operators[index];
                operatorSelect.append($(document.createElement('option')).attr('value', filter.Value).html(filter.Text));
            }
            operatorSelect
                .val(para.Operator)
                .change(function () {
                    if (this.value === 'IsNull' || this.value === 'IsNotNull') {
                        $('span.ui-combobox', $(this).closest('li')).hide();
                        $('select[name$=Value]', $(this).closest('li')).attr('value', '');
                    } else {
                        $('span.ui-combobox', $(this).closest('li')).show();
                    }
                });


            // value
            generateValueElement(para, para.ID);

            if (self.options.isReadOnly === false) {
                roleActionBox.append(deleteButton);
            }

            filterBox.append(filterSelect);
            operatorBox.append(operatorSelect);

            li.append(roleActionBox).append(filterBox).append(operatorBox).append(valueBox).prepend(ruleDragBtn);
            self._dragEvent();

            return li;
        },
        _elementReName: function () {
            var self = this;

            var reName = function (content, objParentName) {
                $('>dl', content).each(function (index, dl) {
                    var conditionVal = $('>dt input:radio:checked', dl).val();
                    $.data(dl, 'value', conditionVal);
                });

                $('>dl', content).each(function (index, dl) {
                    var objName = objParentName.replace(/\[##]/g, '[' + index + ']');
                    $('>dt input:radio', dl).each(function (radioIndex) {
                        var radioName = objName + '.Condition';
                        $(this).attr({ id: radioName + radioIndex, name: radioName });
                    });

                    var value = $.data(dl, 'value');
                    $('>dt input:radio[value="' + value + '"]', dl).attr('checked', 'checked');

                    $('>dd>ul>li', dl).each(function (liIndex, li) {
                        var roleRuleName = objName + '.RuleList[' + liIndex + ']';
                        $('div', li).each(function (divIndex, div) {
                            if ($(div).hasClass('rule-filter-container')) {
                                $('select', div).attr({ id: roleRuleName + '.ID', name: roleRuleName + '.ID' });
                            }
                            if ($(div).hasClass('rule-operator-container')) {
                                $('select', div).attr({ id: roleRuleName + '.Operator', name: roleRuleName + '.Operator' });
                            }
                            if ($(div).hasClass('rule-value-container')) {
                                $('select', div).attr({ id: roleRuleName + '.Value', name: roleRuleName + '.Value' });
                            }
                        });
                    });

                    $('>dd>ul', dl).each(function (ulIndex, ul) {
                        var groupRuleName = objName + '.GroupRuleList[##]';
                        reName(ul, groupRuleName);
                    });
                });
            };

            var objParentName = self.options.modelName;
            reName(self.element, objParentName);
        }
    });

    $.extend($.ui.queryBuilder, {
        locale: {
            DeleteButton: '刪除',
            DeleteGroupRoleButton: '刪除群組',
            AddGroupRoleButton: '新增群組',
            AddRoleRuleButton: '新增規則',
            ANDCondition: '且(AND)',
            ORCondition: '或(OR)'
        },
        operators: [
            { Value: 'Equal', Text: '=' },
            { Value: 'NotEqual', Text: '<>' },
            //{ Value: 'In', Text: 'In' },
            //{ Value: 'NotIn', Text: 'Not In' },
            { Value: 'Less', Text: '<' },
            { Value: 'LessOrEqual', Text: '<=' },
            { Value: 'Greater', Text: '>' },
            { Value: 'GreaterOrEqual', Text: '>=' },
            //{ Value: 'Like', Text: 'Like' },
            { Value: 'IsNull', Text: 'Is Null' },
            { Value: 'IsNotNull', Text: 'Is Not Null' }
        ]
    });

})(jQuery);