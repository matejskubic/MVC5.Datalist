/*!
 * Datalist 5.0.0
 * https://github.com/NonFactors/MVC5.Datalist
 *
 * Copyright © NonFactors
 *
 * Licensed under the terms of the MIT License
 * http://www.opensource.org/licenses/mit-license.php
 */
(function ($) {
    $.widget('mvc.datalist', {
        _create: function () {
            this._initOptions();
            this._initFilters();
            this._initAutocomplete();
            this._initDatalistOpenSpan();

            this._cleanUp();

            this.reload(false);
        },
        _initOptions: function () {
            var e = this.element;
            var o = this.options;

            o.filters = e.attr('data-filters').split(',').filter(Boolean);
            o.hiddenElement = $('#' + e.attr('data-for'))[0];
            o.page = parseInt(e.attr('data-page'));
            o.order = e.attr('data-order');
            o.search = e.attr('data-search');
            o.title = e.attr('data-title');
            o.rows = e.attr('data-rows');
            o.sort = e.attr('data-sort');
            o.url = e.attr('data-url');
        },
        _initFilters: function () {
            for (var i = 0; i < this.options.filters.length; i++) {
                this._initFilter($('[name="' + this.options.filters[i] + '"]'));
            }
        },
        _initFilter: function (filters) {
            var that = this;
            that._on(filters, {
                change: function () {
                    if (that.options.filterChange) {
                        that.options.filterChange(e, that.element[0], that.options.hiddenElement);
                    }

                    if (!e.isDefaultPrevented()) {
                        that._select(null, true);
                    }
                }
            });
        },
        _initAutocomplete: function () {
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
                                };
                            }));
                        }
                    });
                },
                select: function (e, selection) {
                    that._select(selection.item.item, true);
                    e.preventDefault();
                },
                minLength: 1,
                delay: 500
            });

            this.element.on('keyup.datalist', function (e) {
                if (e.which != 9 && this.value.length == 0 && $(that.options.hiddenElement).val()) {
                    that._select(null, true);
                }
            });
            this.element.prevAll('.ui-helper-hidden-accessible').remove();
        },
        _initDatalistOpenSpan: function () {
            var browse = this.element.nextAll('.datalist-browse:first');
            if (browse.length != 0) {
                var that = this;

                this._on(browse, {
                    click: function () {
                        var timeout;
                        datalist
                            .find('.datalist-search')
                            .off('keyup.datalist')
                            .on('keyup.datalist', function (e) {
                                if (that.element.is('[readonly]') || that.element.is('[disabled]')) {
                                    return;
                                }

                                if (e.keyCode < 112 || e.keyCode > 126) {
                                    var input = this;
                                    clearTimeout(timeout);
                                    timeout = setTimeout(function () {
                                        that.options.search = input.value;
                                        that.options.page = 0;
                                        that._update(datalist);
                                    }, 500);
                                }
                            })
                            .val(that.options.search);
                        datalist
                            .find('.datalist-rows input')
                            .spinner({
                                change: function () {
                                    this.value = that._limitTo(this.value, 1, 99);
                                    that.options.rows = this.value;
                                    that.options.page = 0;
                                    that._update(datalist);
                                }
                            })
                            .val(that._limitTo(that.options.rows, 1, 99));

                        datalist.find('.datalist-search').attr('placeholder', $.fn.datalist.lang.Search);
                        datalist.find('.datalist-error').html($.fn.datalist.lang.Error);
                        datalist.dialog('option', 'title', that.options.title);
                        datalist.find('thead').empty();
                        datalist.find('tbody').empty();
                        datalist.find('ul').hide();
                        that._update(datalist);

                        setTimeout(function () {
                            var dialog = datalist.dialog('open').parent();

                            if (parseInt(dialog.css('left')) < 0) {
                                dialog.css('left', 0);
                            }
                            if (parseInt(dialog.css('top')) > 100) {
                                dialog.css('top', '100px');
                            }
                            else if (parseInt(dialog.css('top')) < 0) {
                                dialog.css('top', 0);
                            }
                        }, 100);
                    }
                });
            }
        },

        _formAutocompleteUrl: function (search) {
            return this.options.url +
                '?Search=' + encodeURIComponent(search) +
                '&Sort=' + encodeURIComponent(this.options.sort) +
                '&Order=' + encodeURIComponent(this.options.order) +
                '&Rows=20' +
                this._formFiltersQuery();
        },
        _formDatalistUrl: function (search) {
            return this.options.url +
                '?Search=' + encodeURIComponent(search) +
                '&Sort=' + encodeURIComponent(this.options.sort) +
                '&Order=' + encodeURIComponent(this.options.order) +
                '&Rows=' + encodeURIComponent(this.options.rows) +
                '&Page=' + encodeURIComponent(this.options.page) +
                this._formFiltersQuery();
        },
        _formFiltersQuery: function () {
            var query = '';
            for (var i = 0; i < this.options.filters.length; i++) {
                var filters = $('[name="' + this.options.filters[i] + '"]');
                for (var j = 0; j < filters.length; j++) {
                    query += '&' + encodeURIComponent(this.options.filters[i]) + '=' + encodeURIComponent(filters[j].value);
                }
            }

            return query;
        },

        _defaultSelect: function (data, triggerChanges) {
            if (data) {
                $(this.options.hiddenElement).val(data.DatalistIdKey);
                $(this.element).val(data.DatalistAcKey);
            } else {
                $(this.options.hiddenElement).val(null);
                $(this.element).val(null);
            }

            if (triggerChanges) {
                $(this.options.hiddenElement).change();
                $(this.element).change();
            }
        },
        _select: function (data, triggerChanges) {
            var event = $.Event(this._defaultSelect);
            if (this.options.select) {
                this.options.select(event, this.element[0], this.options.hiddenElement, data, triggerChanges);
            }

            if (!event.isDefaultPrevented()) {
                this._defaultSelect(data, triggerChanges);
            }
        },

        _limitTo: function (value, min, max) {
            return Math.min(Math.max(parseInt(value), min), max) || 20;
        },
        _cleanUp: function () {
            this.element.removeAttr('data-filters');
            this.element.removeAttr('data-search');
            this.element.removeAttr('data-order');
            this.element.removeAttr('data-title');
            this.element.removeAttr('data-rows');
            this.element.removeAttr('data-page');
            this.element.removeAttr('data-sort');
            this.element.removeAttr('data-url');
        },

        _update: function (datalist) {
            var that = this;
            var search = datalist.find('.datalist-search').val();
            datalist.find('.datalist-error').fadeOut(300);

            var timeout = setTimeout(function () {
                datalist.find('.datalist-loading').fadeIn(300);
                datalist.find('table').fadeOut(300);
                datalist.find('ul').fadeOut(300);
            }, 500);

            $.ajax({
                url: that._formDatalistUrl(search),
                cache: false,
                success: function (data) {
                    that._updateHeader(datalist, data.Columns);
                    that._updateData(datalist, data);
                    that._updateNavbar(datalist, data.FilteredRows);

                    clearTimeout(timeout);
                    datalist.find('.datalist-error').hide();
                    datalist.find('.datalist-loading').fadeOut(300);
                    datalist.find('table').fadeIn(300);
                    datalist.find('ul').fadeIn(300);
                },
                error: function () {
                    clearTimeout(timeout);
                    datalist.find('.datalist-error').fadeIn(300);
                    datalist.find('.datalist-loading').hide();
                    datalist.find('table').hide();
                    datalist.find('ul').hide();
                }
            });
        },
        _updateHeader: function (datalist, columns) {
            var sorted = false;
            var that = this;
            var header = '';

            for (var i = 0; i < columns.length; i++) {
                var column = columns[i];
                if (column.Hidden) {
                    continue;
                }

                header += '<th class="' + (column.CssClass || '');
                if (that.options.sort == column.Key || (that.options.sort == '' && !sorted)) {
                    header += 'datalist-' + (that.options.order == 'Asc' ? 'asc' : 'desc');
                    that.options.sort = column.Key;
                    sorted = true;
                }

                header += '" data-column="' + column.Key + '">' + (column.Header || '') + '</th>';
            }

            datalist.find('thead').html('<tr>' + header + '<th></th></tr>');
            datalist.find('th').click(function () {
                var header = $(this);
                if (!header.attr('data-column')) {
                    return false;
                }

                if (that.options.sort == header.attr('data-column')) {
                    that.options.order = that.options.order == 'Asc' ? 'Desc' : 'Asc';
                } else {
                    that.options.order = 'Asc';
                }

                that.options.sort = header.attr('data-column');
                that._update(datalist);
            });
        },
        _updateData: function (datalist, data) {
            if (data.Rows.length == 0) {
                var columns = (data.Columns) ? data.Columns.length + 1 : 1;
                datalist.find('tbody').html('<tr><td colspan="' + columns + '" style="text-align: center">' + $.fn.datalist.lang.NoData + '</td></tr>');

                return;
            }

            var tableData = '';
            for (var i = 0; i < data.Rows.length; i++) {
                var tableRow = '<tr>';
                var row = data.Rows[i];

                for (var j = 0; j < data.Columns.length; j++) {
                    var column = data.Columns[j];
                    if (column.Hidden) {
                        continue;
                    }

                    tableRow += '<td' + (column.CssClass ? ' class="' + column.CssClass + '">' : '>') + (row[column.Key] || '') + '</td>';
                }

                tableRow += '<td></td></tr>';
                tableData += tableRow;
            }

            datalist.find('tbody').html(tableData);
            var selectRows = datalist.find('tbody tr');
            for (var k = 0; k < selectRows.length; k++) {
                this._bindSelect(datalist, selectRows[k], data.Rows[k]);
            }
        },
        _updateNavbar: function (datalist, filteredRows) {
            var pageLength = datalist.find('.datalist-rows input').val();
            var totalPages = parseInt(filteredRows / pageLength) + 1;
            if (filteredRows % pageLength == 0) {
                totalPages--;
            }

            if (totalPages == 0) {
                datalist.find('ul').empty();
            } else {
                this._paginate(totalPages);
            }
        },
        _paginate: function (totalPages) {
            var startingPage = Math.floor(this.options.page / 5) * 5;
            var currentPage = this.options.page;
            var page = startingPage;
            var pagination = '';
            var that = this;

            if (totalPages > 5 && currentPage > 0) {
                pagination = '<li><span data-page="0">&laquo;</span></li><li><span data-page="' + (currentPage - 1) + '">&lsaquo;</span></li>';
            }

            while (page < totalPages && page < startingPage + 5) {
                pagination += '<li' + (page == this.options.page ? ' class="active"' : '') + '><span data-page="' + page + '">' + (++page) + '</span></li>';
            }

            if (totalPages > 5 && currentPage < (totalPages - 1)) {
                pagination += '<li><span data-page="' + (currentPage + 1) + '">&rsaquo;</span></li><li><span data-page="' + (totalPages - 1) + '">&raquo;</span></li>';
            }

            datalist.find('ul').html(pagination).find('li:not(.active) > span').click(function (e) {
                that.options.page = parseInt($(this).data('page'));
                that._update(datalist);
            });
        },
        _bindSelect: function (datalist, selectRow, data) {
            var that = this;
            that._on(selectRow, {
                click: function () {
                    datalist.dialog('close');
                    that._select(data, false);
                }
            });
        },

        reload: function (triggerChanges) {
            var that = this;
            triggerChanges = triggerChanges == null ? true : triggerChanges;

            var id = $(that.options.hiddenElement).val();
            if (id) {
                $.ajax({
                    url: that.options.url + '?id=' + id + '&rows=1' + this._formFiltersQuery(),
                    cache: false,
                    success: function (data) {
                        if (data.rows.length > 0) {
                            that._select(data.rows[0], triggerChanges);
                        }
                    }
                });
            } else {
                that._select(null, triggerChanges);
            }
        },

        _destroy: function () {
            var e = this.element;
            var o = this.options;

            e.attr('data-filters', o.filters.join());
            e.attr('data-search', o.search);
            e.attr('data-order', o.order);
            e.attr('data-title', o.title);
            e.attr('data-page', o.page);
            e.attr('data-rows', o.rows);
            e.attr('data-sort', o.sort);
            e.attr('data-url', o.url);

            e.autocomplete('destroy');

            return this._super();
        }
    });

    $.fn.datalist.lang = {
        Error: 'Error while retrieving records',
        NoData: 'No data found',
        Search: 'Search...'
    };

    var datalist = $('#Datalist');

    $(function () {
        datalist.find('.datalist-rows input').spinner({ min: 1, max: 99 });
        datalist.on('keyup', '.datalist-rows input', function (e) {
            if (e.which == 13) {
                $(this).blur();
            }
        });
 
        datalist.dialog({
            classes: { 'ui-dialog': 'datalist-dialog' },
            dialogClass: 'datalist-dialog',
            autoOpen: false,
            minHeight: 210,
            height: 'auto',
            minWidth: 455,
            width: 'auto',
            modal: true
        });

        $('.datalist-dialog').resizable({
            handles: 'w,e',
            stop: function (event, ui) {
                $(this).css('height', 'auto');
            }
        });

        $('.datalist-input').datalist();
    });
})(jQuery);
