/*!
 * Datalist 5.0.0
 * https://github.com/NonFactors/MVC5.Datalist
 *
 * Copyright © NonFactors
 *
 * Licensed under the terms of the MIT License
 * http://www.opensource.org/licenses/mit-license.php
 */
var MvcDatalistFilter = (function () {
    function MvcDatalistFilter(element) {
        this.page = element.attr('data-page');
        this.rows = element.attr('data-rows');
        this.sort = element.attr('data-sort');
        this.order = element.attr('data-order');
        this.search = element.attr('data-search');
        this.additionalFilters = element.attr('data-filters').split(',').filter(Boolean);
    }

    MvcDatalistFilter.prototype = {
        getQuery: function (search) {
            var filter = $.extend({}, this, search);
            var query = '?search=' + encodeURIComponent(filter.search) +
                '&sort=' + encodeURIComponent(filter.sort) +
                '&order=' + encodeURIComponent(filter.order) +
                '&rows=' + encodeURIComponent(filter.rows) +
                '&page=' + encodeURIComponent(filter.page) +
                (filter.ids ? filter.ids : '');

            for (var i = 0; i < this.additionalFilters.length; i++) {
                var filters = $('[name="' + this.additionalFilters[i] + '"]');
                for (var j = 0; j < filters.length; j++) {
                    query += '&' + encodeURIComponent(this.additionalFilters[i]) + '=' + encodeURIComponent(filters[j].value);
                }
            }

            return query;
        }
    };

    return MvcDatalistFilter;
}());
var MvcDatalistDialog = (function () {
    function MvcDatalistDialog(datalist) {
        this.datalist = datalist;
        this.filter = datalist.filter;
        this.instance = $('#Datalist');
        this.pager = this.instance.find('ul');
        this.table = this.instance.find('table');
        this.tableHead = this.instance.find('thead');
        this.tableBody = this.instance.find('tbody');
        this.title = datalist.element.attr('data-title');
        this.error = this.instance.find('.datalist-error');
        this.search = this.instance.find('.datalist-search');
        this.loader = this.instance.find('.datalist-loading');
        this.rows = this.instance.find('.datalist-rows input');
        this.selector = this.instance.find('.datalist-selector button');

        this.initOptions();
    }

    MvcDatalistDialog.prototype = {
        set: function (options) {
            options = options || {};
            $.extend(this.options.dialog, options.dialog);
            $.extend(this.options.spinner, options.spinner);
            $.extend(this.options.resizable, options.resizable);
        },
        initOptions: function () {
            var dialog = this;

            this.options = {
                dialog: {
                    classes: { 'ui-dialog': 'datalist-dialog' },
                    dialogClass: 'datalist-dialog',
                    title: dialog.title,
                    autoOpen: false,
                    minHeight: 210,
                    minWidth: 455,
                    width: 'auto',
                    modal: true
                },
                spinner: {
                    min: 1,
                    max: 99,
                    change: function () {
                        this.value = dialog.limitRows(this.value);
                        dialog.filter.rows = this.value;
                        dialog.filter.page = 0;

                        dialog.refresh();
                    }
                },
                resizable: {
                    handles: 'w,e',
                    stop: function () {
                        $(this).css('height', 'auto');
                    }
                }
            }
        },

        open: function () {
            this.error.html(this.lang('Error'));
            this.search.val(this.filter.search);
            this.selected = this.datalist.selected.slice();
            this.rows.val(this.limitRows(this.filter.rows));
            this.search.attr('placeholder', this.lang('Search'));
            this.selector.parent().css('display', this.datalist.multi ? '' : 'none');
            this.selector.text(this.lang('Select').replace('{0}', this.datalist.selected.length));

            this.bind();
            this.refresh();

            setTimeout(function (instance) {
                var dialog = instance.dialog('open').parent();

                if (parseInt(dialog.css('left')) < 0) {
                    dialog.css('left', 0);
                }
                if (parseInt(dialog.css('top')) > 100) {
                    dialog.css('top', '100px');
                }
                else if (parseInt(dialog.css('top')) < 0) {
                    dialog.css('top', 0);
                }
            }, 100, this.instance);
        },
        close: function () {
            this.instance.dialog('close');
        },

        refresh: function () {
            var dialog = this;
            this.error.fadeOut(300);
            var loading = setTimeout(function (dialog) {
                dialog.loader.fadeIn(300);
                dialog.table.fadeOut(300);
                dialog.pager.fadeOut(300);
            }, 500, dialog);

            $.ajax({
                cache: false,
                url: dialog.datalist.url + dialog.filter.getQuery() + dialog.selected.map(function (x) { return '&selected=' + x.DatalistIdKey; }).join(''),
                success: function (data) {
                    clearTimeout(loading);
                    dialog.render(data);
                },
                error: function () {
                    clearTimeout(loading);
                    dialog.render();
                }
            });
        },

        render: function (data) {
            this.loader.fadeOut(300);
            this.tableHead.empty();
            this.tableBody.empty();
            this.pager.empty();

            if (data) {
                this.renderHeader(data.Columns);
                this.renderBody(data.Columns, data.Rows);
                this.renderFooter(data.FilteredRows);

                this.table.fadeIn(300);
                this.pager.fadeIn(300);
            } else {
                this.error.fadeIn(300);
            }
        },
        renderHeader: function (columns) {
            var tr = document.createElement('tr');
            var selection = document.createElement('th');

            for (var i = 0; i < columns.length; i++) {
                if (!columns[i].Hidden) {
                    tr.appendChild(this.createHeaderColumn(columns[i]));
                }
            }

            tr.appendChild(selection);
            this.tableHead.append(tr);
        },
        renderBody: function (columns, rows) {
            if (rows.length == 0) {
                var empty = this.createEmptyRow(columns);
                empty.children[0].innerHTML = this.lang('NoData');
                empty.className = 'datalist-empty';

                this.tableBody.append(empty);
            }

            for (var i = 0; i < rows.length; i++) {
                var tr = this.createDataRow(rows[i]);
                var selection = document.createElement('td');

                for (var j = 0; j < columns.length; j++) {
                    if (!columns[j].Hidden) {
                        var td = document.createElement('td');
                        td.className = columns[j].CssClass || '';
                        td.innerText = rows[i][columns[j].Key];

                        tr.appendChild(td);
                    }
                }

                tr.appendChild(selection);
                this.tableBody.append(tr);

                if (i == this.selected.length - 1) {
                    var separator = this.createEmptyRow(columns);
                    separator.className = 'datalist-split';

                    this.tableBody.append(separator);
                }
            }
        },
        renderFooter: function (filteredRows) {
            this.totalRows = filteredRows + this.selected.length;
            var totalPages = Math.ceil(filteredRows / this.filter.rows);

            if (totalPages > 0) {
                var startingPage = Math.floor(this.filter.page / 5) * 5;

                if (totalPages > 5 && this.filter.page > 0) {
                    this.renderPage('&laquo', 0);
                    this.renderPage('&lsaquo;', this.filter.page - 1);
                }

                for (var i = startingPage; i < totalPages && i < startingPage + 5; i++) {
                    this.renderPage(i + 1, i);
                }

                if (totalPages > 5 && this.filter.page < totalPages - 1) {
                    this.renderPage('&rsaquo;', this.filter.page + 1);
                    this.renderPage('&raquo;', totalPages - 1);
                }
            }
        },

        createDataRow: function (data) {
            var dialog = this;
            var datalist = this.datalist;
            var row = document.createElement('tr');
            if (dialog.indexOfSelected(data.DatalistIdKey) >= 0) {
                row.className = 'selected';
            }

            $(row).on('click.datalist', function (e) {
                var index = dialog.indexOfSelected(data.DatalistIdKey);
                if (index >= 0) {
                    dialog.selected.splice(index, 1);

                    $(this).removeClass('selected');
                } else {
                    if (datalist.multi) {
                        dialog.selected.push(data);
                    } else {
                        dialog.selected = [data];
                    }

                    $(this).addClass('selected');
                }

                if (datalist.multi) {
                    dialog.selector.text(dialog.lang('Select').replace('{0}', dialog.selected.length));
                } else {
                    datalist.select(dialog.selected, false);

                    dialog.close();
                }
            });

            return row;
        },
        createEmptyRow: function (columns) {
            var row = document.createElement('tr');
            var td = document.createElement('td');
            row.appendChild(td);

            td.setAttribute('colspan', columns.length + 1);

            return row;
        },
        createHeaderColumn: function (column) {
            var header = document.createElement('th');
            header.innerText = column.Header;
            var filter = this.filter;
            var dialog = this;

            if (column.CssClass) {
                header.className = column.CssClass;
            }

            if (filter.sort == column.Key) {
                header.className += ' datalist-' + filter.order.toLowerCase();
            }

            $(header).on('click.datalist', function () {
                if (filter.sort == column.Key) {
                    filter.order = filter.order == 'Asc' ? 'Desc' : 'Asc';
                } else {
                    filter.order = 'Asc';
                }

                filter.sort = column.Key;
                dialog.refresh();
            });

            return header;
        },
        renderPage: function (text, value) {
            var content = document.createElement('span');
            var page = document.createElement('li');
            page.appendChild(content);
            content.innerHTML = text;
            var dialog = this;

            if (dialog.filter.page == value) {
                page.className = 'active';
            } else {
                $(content).on('click.datalist', function (e) {
                    var expectedPages = Math.ceil((dialog.totalRows - dialog.selected.length) / dialog.filter.rows);
                    if (value < expectedPages) {
                        dialog.filter.page = value;
                    } else {
                        dialog.filter.page = expectedPages - 1;
                    }

                    dialog.refresh();
                });
            }

            dialog.pager.append(page);
        },

        indexOfSelected: function (id) {
            for (var i = 0; i < this.selected.length; i++) {
                if (this.selected[i].DatalistIdKey == id) {
                    return i;
                }
            }

            return -1;
        },
        limitRows: function (value) {
            var spinner = this.options.spinner;

            return Math.min(Math.max(parseInt(value), spinner.min), spinner.max) || this.filter.rows;
        },

        lang: function (key) {
            return $.fn.datalist.lang[key];
        },
        bind: function () {
            var timeout;
            var dialog = this;

            dialog.instance.dialog(dialog.options.dialog);
            dialog.instance.dialog('option', 'close', function () {
                if (dialog.datalist.multi) {
                    dialog.datalist.select(dialog.selected, true);
                }
            });
            dialog.instance.parent().resizable(dialog.options.resizable);

            dialog.search.off('keyup.datalist').on('keyup.datalist', function (e) {
                if (e.keyCode < 112 || e.keyCode > 126) {
                    var input = this;
                    clearTimeout(timeout);
                    timeout = setTimeout(function () {
                        dialog.filter.search = input.value;
                        dialog.filter.page = 0;

                        dialog.refresh();
                    }, 500);
                }
            });

            dialog.rows.spinner(dialog.options.spinner);
            dialog.rows.off('keyup.datalist').on('keyup.datalist', function (e) {
                if (e.which == 13) {
                    this.blur();
                    this.focus();
                }
            });

            dialog.selector.off('click').on('click', function () {
                dialog.close();
            });
        }
    };

    return MvcDatalistDialog;
}());
var MvcDatalist = (function () {
    function MvcDatalist(element, options) {
        this.multi = element.attr('data-multi') == 'true';
        this.filter = new MvcDatalistFilter(element);
        this.for = element.attr('data-for');
        this.url = element.attr('data-url');
        this.selected = [];

        this.element = element;
        this.hiddenElements = $('[name="' + this.for + '"]');
        this.browse = $('.datalist-browse[data-for="' + this.for + '"]');

        this.dialog = new MvcDatalistDialog(this);
        this.initOptions();
        this.set(options);

        this.reload(false);
        this.cleanUp();
        this.bind();
    }

    MvcDatalist.prototype = {
        set: function (options) {
            options = options || {};
            this.dialog.set(options);
            this.events = $.extend(this.events, options.events);
            this.element.autocomplete($.extend(this.options.autocomplete, options.autocomplete));
        },
        initOptions: function () {
            var datalist = this;

            this.options = {
                autocomplete: {
                    source: function (request, response) {
                        $.ajax({
                            url: datalist.url + datalist.filter.getQuery({ search: request.term, rows: 20 }),
                            success: function (data) {
                                response($.map(data.rows, function (item) {
                                    return {
                                        label: item.DatalistAcKey,
                                        value: item.DatalistAcKey,
                                        item: item
                                    };
                                }));
                            }
                        });
                    },
                    select: function (e, selection) {
                        datalist.select([selection.item.item], true);
                        e.preventDefault();
                    },
                    minLength: 1,
                    delay: 500
                }
            };
        },

        reload: function (triggerChanges) {
            var datalist = this;
            var ids = $.grep(datalist.hiddenElements.map(function (i, e) { return encodeURIComponent(e.value); }).get(), Boolean);
            if (ids.length > 0) {
                $.ajax({
                    url: datalist.url + datalist.filter.getQuery({ ids: '&ids=' + ids.join('&ids='), rows: ids.length }),
                    cache: false,
                    success: function (data) {
                        if (data.Rows.length > 0) {
                            var selected = [];
                            for (var i = 0; i < data.Rows.length; i++) {
                                var index = ids.indexOf(data.Rows[i].DatalistIdKey);
                                selected[index] = data.Rows[i];
                            }

                            datalist.select(selected, triggerChanges);
                        }
                    }
                });
            } else {
                datalist.select([], triggerChanges);
            }
        },

        select: function (data, triggerChanges) {
            if (this.events.select) {
                var e = $.Event('select.datalist');
                this.events.select.apply(this, [e, data, triggerChanges]);

                if (e.isDefaultPrevented()) {
                    return;
                }
            }

            this.selected = data;
            if (this.multi) {
                this.hiddenElements.remove();
            }

            if (data.length > 0) {
                if (this.multi) {
                    this.element.val(data.map(function (x) { return '"' + x.DatalistAcKey + '"'; }).join(', '));
                    this.hiddenElements = this.createHiddenElements(data);
                    this.element.before(this.hiddenElements);
                } else {
                    this.hiddenElements.val(data[0].DatalistIdKey);
                    this.element.val(data[0].DatalistAcKey);
                }
            } else {
                this.hiddenElements.val('');
                this.element.val('');
            }

            if (triggerChanges) {
                this.hiddenElements.change();
                this.element.change();
            }
        },

        createHiddenElements: function (data) {
            var elements = [];

            for (var i = 0; i < data.length; i++) {
                var element = document.createElement('input');
                element.className = 'datalist-hidden-input';
                element.setAttribute('type', 'hidden');
                element.setAttribute('name', this.for);
                element.value = data[i].DatalistIdKey;

                elements[i] = element;
            }

            return $(elements);
        },
        cleanUp: function () {
            this.element.removeAttr('data-filters');
            this.element.removeAttr('data-search');
            this.element.removeAttr('data-multi');
            this.element.removeAttr('data-order');
            this.element.removeAttr('data-title');
            this.element.removeAttr('data-page');
            this.element.removeAttr('data-rows');
            this.element.removeAttr('data-sort');
            this.element.removeAttr('data-url');
        },
        bind: function () {
            var datalist = this;

            datalist.element.on('keyup.datalist', function (e) {
                if (e.which != 9 && this.value.length == 0 && datalist.hiddenElements.val()) {
                    datalist.select([], true);
                }
            });

            datalist.browse.on('click.datalist', function (e) {
                if (datalist.element.is('[readonly]') || datalist.element.is('[disabled]')) {
                    return;
                }

                datalist.dialog.open();
            });

            var filters = datalist.filter.additionalFilters;
            for (var i = 0; i < filters.length; i++) {
                $('[name="' + filters[i] + '"]').on('change.datalist', function (e) {
                    if (datalist.events.filterChange) {
                        datalist.events.filterChange.apply(datalist, [e]);
                    }

                    if (!e.isDefaultPrevented()) {
                        datalist.select([], true);
                    }
                });
            }
        }
    };

    return MvcDatalist;
}());

$.fn.datalist = function (options) {
    return this.each(function () {
        if (!$.data(this, 'mvc-datalist')) {
            $.data(this, 'mvc-datalist', new MvcDatalist($(this), options));
        } else if (options) {
            $.data(this, 'mvc-datalist').set(options);
        }
    });
};

$.fn.datalist.lang = {
    Error: 'Error while retrieving records',
    NoData: 'No data found',
    Select: 'Select ({0})',
    Search: 'Search...'
};
