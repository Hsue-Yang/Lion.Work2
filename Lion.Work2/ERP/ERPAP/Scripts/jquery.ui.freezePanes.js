;(function($) {
    $.widget('ui.freezePanes', {
        options: {
            width: '800px',
            height: '600px',
            
            headerRows: 1,
            fixedCols: 0,
            hasHighlight: true,

            colWidths: [],

            onStart: null,
            onFinish: null
        },
        _create: function () {
            this._originalDataTableHeight = $(this.element).height();
            this._tableHeights = $('tr', this.element).map(function(idx, tr) { return $(tr).height(); });
            this.id = this.element.attr('id');
            this.containerId = this.id + '_box';
            this._container = $(document.createElement('DIV')).attr('id', this.containerId);
            this.element.removeAttr('style').wrap(this._container);
            this._parameterInitialize();
            this._containerSetup();
            this._createDomElement();
            this._classSetup();
            this._createHeader();
            this._colGroupSetup();
            this._intosBase();
            this._alignTables();
            this._onscrollBulid();

            this.options.hasHighlight && this._tableHover();

            this.options.onFinish && this.options.onFinish();
        },
        _containerSetup: function() {
            var nonCssProps = ['fixedCols', 'headerRows', 'onStart', 'onFinish', 'colWidths', 'hasHighlight'];
            var container = $('#' + this.containerId);
            var options = this.options;
            for (var p in options) {
                if (options.hasOwnProperty(p)) {
                    if ($.inArray(p, nonCssProps) === -1) {
                        container.css(p, options[p]);
                        delete options[p];
                    }
                }
            }
        },
        _tableHover: function() {
            this._tableHoverSetup(this.sDataTable, this.sFDataTable);
            this._tableHoverSetup(this.sFDataTable, this.sDataTable);
        },
        _tableHoverSetup: function(mainTable, secondTable) {
            if (mainTable) {
                var trs = $('tr', mainTable);
                trs.hover(
                    function() {
                        if (secondTable) {
                            var rowIndex = trs.index(this);
                            $('tr:eq(' + rowIndex + ')', secondTable).addClass('tr-hover');
                        }
                        $(this).addClass('tr-hover');
                    }, function() {
                        if (secondTable) {
                            var rowIndex = trs.index(this);
                            $('tr:eq(' + rowIndex + ')', secondTable).removeClass('tr-hover');
                        }
                        $(this).removeClass('tr-hover');
                    }
                ).click(function() {
                    var rowIndex = trs.index(this);
                    if ($(this).hasClass('tr-selected')) {
                        if (secondTable) {
                            $('tr:eq(' + rowIndex + ')', secondTable).removeClass('tr-selected');
                        }
                        $(this).removeClass('tr-selected');
                    } else {
                        if (secondTable) {
                            $('tr:eq(' + rowIndex + ')', secondTable).addClass('tr-selected');
                        }
                        $(this).addClass('tr-selected');
                    }
                });
            }
        },
        _parameterInitialize: function () {
            var optionHeight = parseInt(this.options.height.replace('px', ''));
            if (optionHeight > this._originalDataTableHeight) {
                this.options.height = this._originalDataTableHeight + 25 + 'px';
            } else {
                this.options.height = optionHeight + 'px';
            }
            
            this.headerRows = parseInt(this.options.headerRows || '1');
            this.fixedCols = parseInt(this.options.fixedCols || '0');
            this.colWidths = this.options.colWidths || [];
            this.options.onStart && this.options.onStart();
        },
        _createDomElement: function() {
            this.BaseContainer = document.createElement('DIV');
            this.FixedHeader = this.BaseContainer.cloneNode(false);
            this.HeaderContainer = this.BaseContainer.cloneNode(false);
            this.Header = this.BaseContainer.cloneNode(false);
            this.FixedDataContainer = this.BaseContainer.cloneNode(false);
            this.FixedData = this.BaseContainer.cloneNode(false);
            this.Data = this.BaseContainer.cloneNode(false);
            this.ColGroup = document.createElement('COLGROUP');

            this.sDataTable = document.getElementById(this.id);
            this.sDataTable.style.margin = '0px';
            if (this.cssSkin) {
                this.sDataTable.className += ' ' + this.cssSkin;
            }
            if (this.sDataTable.getElementsByTagName('COLGROUP').length > 0) {
                this.sDataTable.removeChild(this.sDataTable.getElementsByTagName('COLGROUP')[0]);
            }
            this.sParent = this.sDataTable.parentNode;
            this.sParentHeight = this.sParent.offsetHeight;
            this.sParentWidth = this.sParent.offsetWidth;
            this._container.append(this.BaseContainer);
        },
        _classSetup: function() {
            this.BaseContainer.className = 'BaseContainer';
            this.FixedHeader.className = 'FixedHeader';
            this.HeaderContainer.className = 'HeaderContainer';
            this.Header.className = 'Header';
            this.FixedDataContainer.className = 'FixedDataContainer';
            this.FixedData.className = 'FixedData';
            this.Data.className = 'Data';
        },
        _createHeader: function() {
            var alpha, beta, i, self = this;
            self.sHeaderTable = self.sDataTable.cloneNode(false);
            self.sHeaderTable.removeAttribute('id');
            if (self.sDataTable.tHead) {
                alpha = self.sDataTable.tHead;
                self.sHeaderTable.appendChild(alpha.cloneNode(false));
                beta = self.sHeaderTable.tHead;
            } else {
                alpha = self.sDataTable.tBodies[0];
                self.sHeaderTable.appendChild(alpha.cloneNode(false));
                beta = self.sHeaderTable.tBodies[0];
            }
            alpha = alpha.rows;
            for (i = 0; i < self.headerRows; i++) {
                beta.appendChild(alpha[i].cloneNode(true));
            }
            self.Header.appendChild(self.sHeaderTable);

            if (self.fixedCols > 0) {
                self.sFHeaderTable = self.sHeaderTable.cloneNode(true);
                self.FixedHeader.appendChild(self.sFHeaderTable);
                
                $('tr', self.sDataTable).each(function (idx, el) {
                    $(el).css('height', self._tableHeights[idx] + 'px');
                    $('tr:eq(' + idx + ')', self.sFHeaderTable).css('height', self._tableHeights[idx] + 'px');
                    $('tr:eq(' + idx + ')', self.sHeaderTable).css('height', self._tableHeights[idx] + 'px');
                });

                self.sFDataTable = self.sDataTable.cloneNode(true);
                self.sFDataTable.removeAttribute('id');

                $('tr', self.sFDataTable).each(function (idx, el) {
                    $(el).css('height', self._tableHeights[idx] + 'px');
                    $('td:eq(' + (self.fixedCols - 1) + ')', el).nextAll().remove();
                });
                self.FixedData.appendChild(self.sFDataTable);
            }
        },
        _colGroupSetup: function() {
            var clean, i, j, k, m;
            var alpha = [];
            if (this.sDataTable.tBodies[0].rows.length > 0) {
                alpha = this.sDataTable.tBodies[0].rows;
            } else {
                alpha = this.sDataTable.tHead.rows;
            }
            for (i = 0, j = alpha.length; i < j; i++) {
                clean = true;
                for (k = 0, m = alpha[i].cells.length; k < m; k++) {
                    if (alpha[i].cells[k].colSpan !== 1 || alpha[i].cells[k].rowSpan !== 1) {
                        i += alpha[i].cells[k].rowSpan - 1;
                        clean = false;
                        break;
                    }
                }
                if (clean === true) break;
            }
            this.cleanRow = (clean === true) ? i : 0;
            
            if (alpha.length > 0) {
                for (i = 0, j = alpha[this.cleanRow].cells.length; i < j; i++) {
                    if (i === this.colWidths.length || this.colWidths[i] === -1) {
                        this.colWidths[i] = alpha[this.cleanRow].cells[i].offsetWidth;
                    }
                }
            }
            
            for (i = 0, j = this.colWidths.length; i < j; i++) {
                this.ColGroup.appendChild(document.createElement('COL'));
                this.ColGroup.lastChild.setAttribute('width', this.colWidths[i]);
            }
            this.sDataTable.insertBefore(this.ColGroup.cloneNode(true), this.sDataTable.firstChild);
            this.sHeaderTable.insertBefore(this.ColGroup.cloneNode(true), this.sHeaderTable.firstChild);
            if (this.fixedCols > 0) {
                this.sFDataTable.insertBefore(this.ColGroup.cloneNode(true), this.sFDataTable.firstChild);
                this.sFHeaderTable.insertBefore(this.ColGroup.cloneNode(true), this.sFHeaderTable.firstChild);
            }
        },
        _intosBase: function() {
            if (this.fixedCols > 0) {
                this.BaseContainer.appendChild(this.FixedHeader);
            }
            this.HeaderContainer.appendChild(this.Header);
            this.BaseContainer.appendChild(this.HeaderContainer);
            if (this.fixedCols > 0) {
                this.FixedDataContainer.appendChild(this.FixedData);
                this.BaseContainer.appendChild(this.FixedDataContainer);
            }
            this.BaseContainer.appendChild(this.Data);
            this.sParent.insertBefore(this.BaseContainer, this.sDataTable);
            this.Data.appendChild(this.sDataTable);
        },
        _alignTables: function() {
            var alpha, beta;
            var sDataStyles, sDataTableStyles;
            this.sHeaderHeight = 0;
            if (this.sDataTable.tBodies[0].rows.length > 0) {
                this.sHeaderHeight = this.sDataTable.tBodies[0].rows[(this.sDataTable.tHead) ? 0 : this.headerRows].offsetTop;
            }
            sDataTableStyles = 'margin-top: ' + (this.sHeaderHeight * -1) + 'px;';
            sDataTableStyles += 'margin-left: 0px;';
            sDataStyles = 'margin-top: ' + this.sHeaderHeight + 'px;';
            sDataStyles += 'height: ' + (this.sParentHeight - this.sHeaderHeight) + 'px;';
            if (this.fixedCols > 0 && this.sDataTable.tBodies[0].rows.length > 0) {
                this.sFHeaderWidth = this.sDataTable.tBodies[0].rows[this.cleanRow].cells[this.fixedCols].offsetLeft;
                if (window.getComputedStyle) {
                    alpha = document.defaultView;
                    beta = this.sDataTable.tBodies[0].rows[0].cells[0];
                    if (navigator.taintEnabled) {
                        this.sFHeaderWidth += Math.ceil(parseInt(alpha.getComputedStyle(beta, null).getPropertyValue('border-right-width')) / 2);
                    } else {
                        this.sFHeaderWidth += parseInt(alpha.getComputedStyle(beta, null).getPropertyValue('border-right-width'));
                    }
                } else if ( /*@cc_on!@*/0) {
                    alpha = this.sDataTable.tBodies[0].rows[0].cells[0];
                    beta = [alpha.currentStyle['borderRightWidth'], alpha.currentStyle['borderLeftWidth']];
                    if (/px/i.test(beta[0]) && /px/i.test(beta[1])) {
                        beta = [parseInt(beta[0]), parseInt(beta[1])].sort();
                        this.sFHeaderWidth += Math.ceil(parseInt(beta[1]) / 2);
                    }
                }
                
                if (window.opera) {
                    this.FixedDataContainer.style.height = this.sParentHeight + 'px';
                }
                
                this.FixedHeader.style.width = this.sFHeaderWidth + 'px';
                this.FixedData.style.width = $(this.sFHeaderTable).width() + 'px';
                sDataTableStyles += 'margin-left: ' + (this.sFHeaderWidth * -1) + 'px;';
                sDataStyles += 'margin-left: ' + this.sFHeaderWidth + 'px;';
                sDataStyles += 'width: ' + (this.sParentWidth - this.sFHeaderWidth) + 'px;';
            } else {
                sDataStyles += 'width: ' + this.sParentWidth + 'px;';
            }
            this.Data.style.cssText = sDataStyles;
            this.sDataTable.style.cssText = sDataTableStyles;
        },
        _onscrollBulid: function() {
            var self = this;
            if (self.fixedCols > 0) {
                self.Data.onscroll = function() {
                    self.Header.style.right = self.Data.scrollLeft + 'px';
                    self.FixedData.style.top = (self.Data.scrollTop * -1) + 'px';
                };
            } else {
                self.Data.onscroll = function() {
                    self.Header.style.right = self.Data.scrollLeft + 'px';
                };
            }
            if ( /*@cc_on!@*/0) {
                window.attachEvent('onunload', function() {
                    self.Data.onscroll = null;
                });
            }
        },
        HideColumnIndexs: function (columns) {
            this._toggleColumnIndexs(false, columns);
        },
        ShowColumnIndexs: function (columns) {
            this._toggleColumnIndexs(true, columns);
        },
        _toggleColumnIndexs: function (isShow, columns) {
            if ($.isArray(columns)) {
                var self = this;
                var hideColumn = function (index, tr) {
                    if (isShow) {
                        $('td:eq(' + index + '),th:eq(' + index + ')', tr).show();
                    } else {
                        $('td:eq(' + index + '),th:eq(' + index + ')', tr).hide();
                    }
                };
                var hideCol = function(index, tr) {
                    if (isShow) {
                        $('col:eq(' + index + ')', tr).show();
                    } else {
                        $('col:eq(' + index + ')', tr).hide();
                    }
                };
                for (var i = 0; i < columns.length; i++) {
                    $('tr', self.sFDataTable).each(function (idex, tr) { hideColumn(columns[i], tr); });
                    $('tr', self.sDataTable).each(function (idex, tr) { hideColumn(columns[i], tr); });
                    $('tr', self.sHeaderTable).each(function (idex, tr) { hideColumn(columns[i], tr); });
                    $('tr', self.sFHeaderTable).each(function (idex, tr) { hideColumn(columns[i], tr); });

                    $('colgroup', self.sFDataTable).each(function (idex, tr) { hideCol(columns[i], tr); });
                    $('colgroup', self.sDataTable).each(function (idex, tr) { hideCol(columns[i], tr); });
                    $('colgroup', self.sHeaderTable).each(function (idex, tr) { hideCol(columns[i], tr); });
                    $('colgroup', self.sFHeaderTable).each(function (idex, tr) { hideCol(columns[i], tr); });
                }
            }
        }
    });
})(jQuery);