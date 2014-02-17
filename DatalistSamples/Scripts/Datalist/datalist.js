/*!
 * Datalist 3.0.0 beta
 * https://github.com/Muchiachio/MVC.Datalist
 *
 * Copyright (c) 2014 Aldis Rameika
 *
 * Licensed under the terms of the MIT License
 * http://www.opensource.org/licenses/mit-license.php
 */
(function ($) {
    $.widget("mvc.datalist", {
        _create: function () {
            if (!this.element.hasClass('datalist-input')) return;
            
            this._initOptions();
            this._initAdditionalFilters();
            this._createAutocomplete();
            this._loadSelected();
            this._bindDatalist(); // TODO: Fix null values in javascript html code
            this._cleanUp(); // TODO: Fix resizing on different datalists.
        },
        _initOptions: function () {
            this.options.recordsPerPage = this._limitTo(this.element.attr('data-datalist-records-per-page'), 1, 99);
            this.options.hiddenElement = $('#' + this.element.attr('data-datalist-hidden-input'))[0];
            this.options.filters = this.element.attr('data-datalist-filters').split(',');
            this.options.sortColumn = this.element.attr('data-datalist-sort-column');
            this.options.sortOrder = this.element.attr('data-datalist-sort-order');
            this.options.title = this.element.attr('data-datalist-dialog-title');
            this.options.url = this.element.attr('data-datalist-url');
            this.options.term = '';
            this.options.page = 0;
        },
        _initAdditionalFilters: function () {
            var that = this;
            var filters = $.grep(this.options.filters, function (item) { return (item != null && item != ''); });
            for (i = 0; i < filters.length; i++) {
                $('#' + filters[i]).change(function () {
                    var event = $.Event(that._defaultFilterChange);
                    if (that.options.filterChange)
                        that.options.filterChange(event, that.element[0], that.options.hiddenElement, this);
                    if (!event.isDefaultPrevented())
                        that._defaultFilterChange(this);
                });
            }
        },
        _defaultSelect: function(element, data) {
            if (data) {
                $(this.options.hiddenElement).val(data.DatalistIdKey).change();
                $(element).val(data.DatalistAcKey).change();
            }
            else {
                $(element).val(null).change();
                $(this.options.hiddenElement).val(null).change();
            }
        },
        _defaultFilterChange: function (filter) {
            var event = $.Event(this._defaultSelect);
            if (this.options.select)
                this.options.select(event, this.element[0], this.options.hiddenElement, null);
            if (!event.isDefaultPrevented())
                this._defaultSelect(this.element[0], null);
        },
        _createAutocomplete: function () {
            var that = this;
            this.element.autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: that._formAutocompleteUrl(request.term),
                        success: function (data) {
                            response($.map(data.Rows, function (item) {
                                return {
                                    label: item.DatalistAcKey,
                                    value: item.DatalistAcKey,
                                    item: item
                                }
                            }));
                        }
                    });
                },
                select: function (e, selection) {
                    var event = $.Event(that._defaultSelect);
                    if (that.options.select)
                        that.options.select(event, that.element[0], that.options.hiddenElement, selection.item.item);
                    if (!event.isDefaultPrevented())
                        that._defaultSelect(that.element[0], selection.item.item);
                },
                minLength: 1
            });

            this.element.bind('keyup.datalist', function () {
                if (this.value.length == 0) {
                    var event = $.Event(that._defaultSelect);
                    if (that.options.select)
                        that.options.select(event, this, that.options.hiddenElement, null);
                    if (!event.isDefaultPrevented())
                        that._defaultSelect(this, null);
                }
            });
            this.element.prevAll('.ui-helper-hidden-accessible').remove();
        },
        _formAutocompleteUrl: function (term) {
            return this.options.url +
                '?SearchTerm=' + term +
                '&RecordsPerPage=20' +
                '&SortOrder=Asc' +
                '&Page=0' +
                this._formAdditionalFiltersQuery();
        },
        _formDatalistUrl: function (term) {
            return this.options.url +
                '?SearchTerm=' + term +
                '&RecordsPerPage=' + this.options.recordsPerPage +
                '&SortColumn=' + this.options.sortColumn +
                '&SortOrder=' + this.options.sortOrder +
                '&Page=' + this.options.page +
                this._formAdditionalFiltersQuery();
        },
        _formAdditionalFiltersQuery: function() {
            var filters = $.grep(this.options.filters, function (item) { return (item != null && item != ''); });

            var additionaFilter = '';
            for (index = 0; index < filters.length; index++) {
                var value = $('#' + filters[index]).val();
                additionaFilter += '&' + filters[index] + '=' + (value != null ? value : '');
            }

            return additionaFilter;
        },
        _loadSelected: function () {
            var that = this;
            var id = $(this.options.hiddenElement).val();
            if (id != '' && id != 0) {
                $.ajax({
                    url: that.options.url + '?Id=' + id + '&RecordsPerPage=1',
                    cache: false,
                    success: function (data) {
                        if (data.Rows.length > 0) {
                            var event = $.Event(that._defaultSelect);
                            if (that.options.select)
                                that.options.select(event, that.element[0], that.options.hiddenElement, data.Rows[0]);
                            if (!event.isDefaultPrevented())
                                that._defaultSelect(that.element[0], data.Rows[0]);
                        }
                    }
                });
            }
        },
        _bindDatalist: function () {
            var datalistAddon = this.element.nextAll('.datalist-open-span:first');
            if (datalistAddon.length != 0) {
                var datalist = $('#Datalist');
                var that = this;

                this._on(datalistAddon, {
                    click: function () {
                        datalist
                            .find('.datalist-search-input')
                            .unbind('keyup.datalist')
                            .bindWithDelay('keyup.datalist', function () {
                                that.options.term = this.value;
                                that.options.page = 0;
                                that._update(datalist);
                            }, 500, false)
                            .val(that.options.term);
                        datalist
                            .find('.datalist-items-per-page')
                            .spinner({
                                change: function () {
                                    that.options.page = 0;
                                    this.value = that._limitTo(this.value, 1, 99);
                                    that.options.recordsPerPage = this.value;
                                    that._update(datalist);
                                }
                            })
                            .val(that._limitTo(that.options.recordsPerPage, 1, 99));

                        datalist.find('.datalist-search-input').attr('placeholder', $.fn.datalist.lang.Search);
                        datalist.find('.datalist-error-span').html($.fn.datalist.lang.Error);
                        datalist.dialog('option', 'title', this.options.title);
                        datalist.dialog('open');
                        that._update(datalist);
                    }
                });
            }
        },
        _cleanUp: function () {
            this.element.removeAttr('data-datalist-records-per-page')
            this.element.removeAttr('data-datalist-dialog-title');
            this.element.removeAttr('data-datalist-hidden-input');
            this.element.removeAttr('data-datalist-sort-column');
            this.element.removeAttr('data-datalist-sort-order');
            this.element.removeAttr('data-datalist-filters');
            this.element.removeAttr('data-datalist-url');
        },

        _update: function (datalist) {
            var that = this;
            var term = datalist.find('.datalist-search-input').val();

            datalist.find('.datalist-error').fadeOut(300);
            var timeOut = setTimeout(function () {
                datalist.find('.datalist-processing').fadeIn(300);
                datalist.find('.datalist-data').fadeOut(300);
            }, 500);

            $.ajax({
                url: that._formDatalistUrl(term),
                cache: false,
                success: function (data) {
                    that._updateTable(datalist, data);

                    clearTimeout(timeOut);
                    datalist.find('.datalist-processing').fadeOut(300);
                    datalist.find('.datalist-pager').fadeIn(300);
                    datalist.find('.datalist-data').fadeIn(300);
                },
                error: function () {
                    clearTimeout(timeOut);
                    datalist.find('.datalist-processing').fadeOut(300);
                    datalist.find('.datalist-pager').fadeOut(300);
                    datalist.find('.datalist-data').fadeOut(300);
                    datalist.find('.datalist-error').fadeIn(300);
                }
            });
        },
        _updateTable: function (datalist, data) {
            this._updateTableHeader(datalist, data.Columns);
            this._updateTableData(datalist, data);
            this._updateNavigationBar(datalist, data);
        },
        _updateTableHeader: function (datalist, columns) {
            var that = this;
            var header = '';
            var columnCount = 0;
            for (var key in columns) {
                header += '<th data-column="' + key + '">' + columns[key];
                if (that.options.sortColumn == key || (that.options.sortColumn == '' && columnCount == 0)) {
                    that.options.sortColumn = key;
                    header += '<span class="datalist-sort-arrow glyphicon glyphicon-arrow-' + (that.options.sortOrder == 'Asc' ? 'down' : 'up') + '"></span>';
                }

                header += '</th>';
                columnCount++;
            }

            datalist.find('.datalist-table-head').html('<tr>' + header + '<th class="datalist-select-header"></th></tr>');
            datalist.find('.datalist-table-head th').click(function () {
                var header = $(this);
                if (!header.attr('data-column')) return false;
                if (that.options.sortColumn == header.attr('data-column'))
                    that.options.sortOrder = that.options.sortOrder == 'Asc' ? 'Desc' : 'Asc';
                else
                    that.options.sortOrder = 'Asc';

                that.options.sortColumn = header.attr('data-column');
                that._update(datalist);
            });
        },
        _updateTableData: function (datalist, data) {
            if (data.Rows.length == 0) {
                datalist.find('.datalist-table-body').html('<tr><td colspan="0" style="text-align: center">' + $.fn.datalist.lang.NoDataFound + '</tr>');
                return;
            }

            var tableData = '';
            for (var i = 0; i < data.Rows.length; i++) {
                var tableRow = '<tr>'
                var row = data.Rows[i];
                for (var key in data.Columns)
                    tableRow += '<td>' + row[key] + '</td>';

                tableRow += '<td class="datalist-select-cell"><div class="datalist-select-container"><i class="glyphicon glyphicon-ok"></div></i></td></tr>';
                tableData += tableRow;
            }

            datalist.find('.datalist-table-body').html(tableData);
            var selectCells = datalist.find('td.datalist-select-cell');
            for (var i = 0; i < selectCells.length; i++) {
                this._bindSelect(datalist, selectCells[i], data.Rows[i]);
            }
        },
        _updateNavigationBar: function (datalist, data) {
            var that = this;
            var pageLength = datalist.find('.datalist-items-per-page').val();
            var totalPages = parseInt(data.FilteredRecords / pageLength) + 1;
            if (data.FilteredRecords % pageLength == 0)
                totalPages--;

            if (totalPages == 0)
                datalist.find('.datalist-pager > .pagination').empty();
            else
                datalist.find('.datalist-pager > .pagination').bootstrapPaginator({
                    currentPage: that.options.page + 1,
                    bootstrapMajorVersion: 3,
                    totalPages: totalPages,
                    onPageChanged: function (e, oldPage, newPage) {
                        that.options.page = newPage - 1;
                        that._update(datalist);
                    },
                    tooltipTitles: function (type, page, current) {
                        return "";
                    },
                    itemTexts: function (type, page, current) {
                        switch (type) {
                            case "first":
                                return "&laquo;";
                            case "prev":
                                return "&lsaquo;";
                            case "next":
                                return "&rsaquo;";
                            case "last":
                                return "&raquo;";
                            case "page":
                                return page;
                        }
                    }
                });
        },
        _bindSelect: function (datalist, selectCell, data) {
            var that = this;
            this._on(selectCell, {
                'click': function () {
                    datalist.dialog('close');
                    var event = $.Event(that._defaultSelect);
                    if (that.options.select)
                        that.options.select(event, that.element[0], that.options.hiddenElement, data);
                    if (!event.isDefaultPrevented())
                        that._defaultSelect(that.element[0], data);
                }
            });
        },

        _limitTo: function(value, min, max) {
            value = parseInt(value);
            if (isNaN(value))
                return 20;
            if (value < 1)
                return 1;
            if (value > 99)
                return 99;

            return value;
        },

        _destroy: function () {
            this.element.attr('data-datalist-records-per-page', this.options.recordsPerPage);
            this.element.attr('data-datalist-hidden-input', this.options.hiddenElement.id);
            this.element.attr('data-datalist-filters', this.options.filters.join());
            this.element.attr('data-datalist-sort-column', this.options.sortColumn);
            this.element.attr('data-datalist-sort-order', this.options.sortOrder);
            this.element.attr('data-datalist-dialog-title', this.options.title);
            this.element.attr('data-datalist-url', this.options.url);
            // TODO: unbind everything
            return this._super();
        }
    });
})(jQuery);

(function ($) {
    $.fn.datalist.lang = {
        Error: "Error while retrieving records",
        NoDataFound: "No data found",
        Search: "Search..."
    };

    var datalist = $('#Datalist');
    datalist.find('.datalist-items-per-page').spinner({ min: 1, max: 99 }).parent().addClass('input-group-addon');
    datalist.dialog({
        autoOpen: false,
        minHeight: 210,
        height: 'auto',
        minWidth: 455,
        width: 'auto',
        modal: true
    });
}(jQuery));