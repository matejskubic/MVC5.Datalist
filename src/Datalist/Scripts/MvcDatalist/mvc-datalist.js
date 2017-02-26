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
                (filter.id ? '&id=' + encodeURIComponent(filter.id) : '');

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
        this.error = this.instance.find('.datalist-error');
        this.search = this.instance.find('.datalist-search');
        this.loader = this.instance.find('.datalist-loading');
        this.rows = this.instance.find('.datalist-rows input');

        this.instance.dialog({
            classes: { 'ui-dialog': 'datalist-dialog' },
            dialogClass: 'datalist-dialog',
            autoOpen: false,
            minHeight: 210,
            minWidth: 455,
            width: 'auto',
            modal: true
        }).parent().resizable({
            handles: 'w,e',
            stop: function (event, ui) {
                $(this).css('height', 'auto');
            }
        });
    }

    MvcDatalistDialog.prototype = {
        open: function () {
            this.error.html(this.lang('Error'));
            this.search.val(this.filter.search);
            this.search.attr('placeholder', this.lang('Search'));
            this.rows.val(this.limitTo(this.filter.rows, 1, 99));
            this.instance.dialog('option', 'title', this.datalist.title);

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
                url: dialog.datalist.url + dialog.filter.getQuery(),
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

            if (!this.filter.sort && columns.length > 0) {
                tr.children[0].className += ' datalist-' + this.filter.order.toLowerCase();
            }

            tr.appendChild(selection);
            this.tableHead.append(tr);
        },
        renderBody: function (columns, rows) {
            if (rows.length == 0) {
                var empty = document.createElement('tr');
                var td = document.createElement('td');
                empty.appendChild(td);

                td.setAttribute('colspan', columns.length + 1);
                td.innerHTML = this.lang('NoData');
                td.className = 'datalist-empty';

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
            }
        },
        renderFooter: function (filteredRows) {
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
            var row = document.createElement('tr');
            $(row).on('click.datalist', function (e) {
                dialog.datalist.select(data, true);

                dialog.close();
            });

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
                    dialog.filter.page = value;

                    dialog.refresh();
                });
            }

            dialog.pager.append(page);
        },

        limitTo: function (value, min, max) {
            return Math.min(Math.max(parseInt(value), min), max) || 20;
        },

        lang: function (key) {
            return $.fn.datalist.lang[key];
        },
        bind: function () {
            var timeout;
            var dialog = this;
            var filter = this.filter;

            this.search.off('keyup.datalist').on('keyup.datalist', function (e) {
                if (e.keyCode < 112 || e.keyCode > 126) {
                    var input = this;
                    clearTimeout(timeout);
                    timeout = setTimeout(function () {
                        filter.search = input.value;
                        filter.page = 0;

                        dialog.refresh();
                    }, 500);
                }
            });

            this.rows.spinner({
                min: 1,
                max: 99,
                change: function () {
                    this.value = dialog.limitTo(this.value, 1, 99);
                    filter.rows = this.value;
                    filter.page = 0;

                    dialog.refresh();
                }
            }).off('keyup.datalist').on('keyup.datalist', function (e) {
                if (e.which == 13) {
                    this.blur();
                    this.focus();
                }
            });
        }
    };

    return MvcDatalistDialog;
}());
var MvcDatalist = (function () {
    function MvcDatalist(element, options) {
        this.url = element.attr('data-url');
        this.title = element.attr('data-title');
        this.filter = new MvcDatalistFilter(element);

        this.element = element;
        this.hiddenElement = $('#' + element.attr('data-for'));
        this.browse = $('.datalist-browse[data-for="' + element.attr('data-for') + '"]');

        this.dialog = new MvcDatalistDialog(this);
        this.events = {};

        this.set(options);

        this.reload(false);
        this.cleanUp();
        this.bind();
    }

    MvcDatalist.prototype = {
        set: function (options) {
            options = options || {};
            this.events.select = options.select || this.events.select;
            this.events.filterChange = options.filterChange || this.events.filterChange;
        },
        reload: function (triggerChanges) {
            var datalist = this;
            var id = datalist.hiddenElement.val();

            if (id) {
                $.ajax({
                    url: datalist.url + datalist.filter.getQuery({ id: id, rows: 1 }),
                    cache: false,
                    success: function (data) {
                        if (data.Rows.length > 0) {
                            datalist.select(data.Rows[0], triggerChanges);
                        }
                    }
                });
            } else {
                datalist.select(null, triggerChanges);
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

            this.hiddenElement.val(data ? data.DatalistIdKey : '');
            this.element.val(data ? data.DatalistAcKey : '');

            if (triggerChanges) {
                this.hiddenElement.change();
                this.element.change();
            }
        },

        cleanUp: function () {
            this.element.removeAttr('data-filters');
            this.element.removeAttr('data-search');
            this.element.removeAttr('data-order');
            this.element.removeAttr('data-title');
            this.element.removeAttr('data-page');
            this.element.removeAttr('data-rows');
            this.element.removeAttr('data-sort');
            this.element.removeAttr('data-url');
        },
        bind: function () {
            var datalist = this;

            datalist.element.autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: datalist.url + datalist.filter.getQuery({ search: request.term, rows: 20 }),
                        success: function (data) {
                            response($.map(data.Rows, function (item) {
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
                    datalist.select(selection.item.item, true);
                    e.preventDefault();
                },
                minLength: 1,
                delay: 500
            }).on('keyup.datalist', function (e) {
                if (e.which != 9 && this.value.length == 0 && datalist.hiddenElement.val()) {
                    datalist.select(null, true);
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
                        datalist.select(null, true);
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
        } else {
            $.data(this, 'mvc-datalist').set(options);
        }
    });
};

$.fn.datalist.lang = {
    Error: 'Error while retrieving records',
    NoData: 'No data found',
    Search: 'Search...'
};
