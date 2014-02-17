test('Init options', 11, function() {
    var datalistInput = $('#DatalistInitOptions');
    datalistInput.attr('data-datalist-url', 'http://localhost:9140/Test');
    datalistInput.attr('data-datalist-dialog-title', 'TestTitle');
    datalistInput.attr('data-datalist-sort-column', 'TestColumn');
    datalistInput.attr('data-datalist-hidden-input', 'InitOptions');
    datalistInput.attr('data-datalist-records-per-page', 30);
    datalistInput.attr('data-datalist-sort-order', 'Desc');
    datalistInput.attr('data-datalist-filters', 'A,B,C');

    datalistInput.datalist();

    equal(datalistInput.datalist('option', 'page'), 0);
    equal(datalistInput.datalist('option', 'term'), '');
    equal(datalistInput.datalist('option', 'select'), null);
    equal(datalistInput.datalist('option', 'sortOrder'), 'Desc');
    equal(datalistInput.datalist('option', 'title'), 'TestTitle');
    equal(datalistInput.datalist('option', 'filterChange'), null);
    equal(datalistInput.datalist('option', 'recordsPerPage'), 30);
    equal(datalistInput.datalist('option', 'sortColumn'), 'TestColumn');
    equal(datalistInput.datalist('option', 'url'), 'http://localhost:9140/Test');
    equal(datalistInput.datalist('option', 'filters').join(), ['A', 'B', 'C'].join());
    equal(datalistInput.datalist('option', 'hiddenElement'), $('#InitOptions')[0]);
});

test('Init options limit records per page', 3, function () {
    var datalistInput = $('#DatalistInitOptionsLimit');
    datalistInput.attr('data-datalist-records-per-page', 'NaN');
    datalistInput.datalist();

    equal(datalistInput.datalist('option', 'recordsPerPage'), 20);

    datalistInput.datalist('destroy');
    datalistInput.attr('data-datalist-records-per-page', -1);
    datalistInput.datalist();

    equal(datalistInput.datalist('option', 'recordsPerPage'), 1);

    datalistInput.datalist('destroy');
    datalistInput.attr('data-datalist-records-per-page', 100);
    datalistInput.datalist();

    equal(datalistInput.datalist('option', 'recordsPerPage'), 99);
});

test('Additional filters binding', 8, function () {
    $('#DatalistFilters').attr('data-datalist-filters', 'Filter1,Filter2');
    var filters = [ $('#Filter1')[0], $('#Filter2')[0] ];
    
    $('#DatalistFilters').datalist({
        filterChange: function (e, element, hiddenElement, filter) {
            equal(element, $('#DatalistFilters')[0])
            equal(hiddenElement, $('#Filters')[0]);
            equal(filter, filters[0]);
            ok(e);

            filters.splice(0, 1);
        }
    });

    $('#Filter1').change();
    $('#Filter2').change();
});

test('Custom filter change', 2, function () {
    $('#DatalistCustomFilterChange').attr('data-datalist-filters', 'CustomFilter');
    $('#DatalistCustomFilterChange').datalist({
        filterChange: function (e, element, hiddenElement, filter) {
            hiddenElement.value = 'Test';
            element.value = 'Test';
            e.preventDefault();
        },
        select: function () {
            ok(false);
        }
    });

    $('#DatalistCustomFilterChange').val(1);
    $('#CustomFilterChange').val(1);
    $('#CustomFilter').change();

    equal($('#DatalistCustomFilterChange').val(), 'Test');
    equal($('#CustomFilterChange').val(), 'Test');
});

test('Custom select for filter change', 5, function () {
    $('#DatalistCustomSelectFilterChange').attr('data-datalist-filters', 'CustomSelectFilter');
    $('#DatalistCustomSelectFilterChange').datalist({
        select: function (e, element, hiddenElement, data) {
            e.preventDefault();
            equal(element, $('#DatalistCustomSelectFilterChange')[0])
            equal(hiddenElement, $('#CustomSelectFilterChange')[0]);
            equal(data, null);

            element.value = 'Test';
            hiddenElement.value = 'Test';
        }
    });

    $('#DatalistCustomSelectFilterChange').val(1);
    $('#CustomSelectFilterChange').val(1);
    $('#CustomSelectFilter').change();

    equal($('#DatalistCustomSelectFilterChange').val(), 'Test');
    equal($('#CustomSelectFilterChange').val(), 'Test');
});

test('Default filter change', 2, function () {
    $('#DatalistDefaultFilterChange').attr('data-datalist-filters', 'DefaultFilter');
    $('#DatalistDefaultFilterChange').datalist({
        select: function () { },
        filterChange: function () { }
    });

    $('#DatalistDefaultFilterChange').val(1);
    $('#DefaultFilterChange').val(1);
    $('#DefaultFilter').change();

    equal($('#DatalistDefaultFilterChange').val(), '');
    equal($('#DefaultFilterChange').val(), '');
});
// TODO: Somehow test autocomplete source method
test('Creates autocomplete', 2, function () {
    ok($('#DatalistAutocomplete').datalist().hasClass('ui-autocomplete-input'));
    equal($('#DatalistAutocomplete').autocomplete('option', 'minLength'), 1)
});

test('Autocomplete select', 7, function () {
    $('#DatalistAutocompleteSelect').datalist({
        select: function (e, element, hiddenElement, data) {
            equal(element, $('#DatalistAutocompleteSelect')[0]);
            equal(hiddenElement, $('#AutocompleteSelect')[0]);
            equal(data.DatalistIdKey, 'Test2');
            equal(data.DatalistAcKey, 'Test3');
            ok(e);
        }
    });

    $('#DatalistAutocompleteSelect').data('ui-autocomplete')
        ._trigger('select', 'autocompleteselect', {
            item: {
                label: 'Test0',
                value: 'Test1',
                item: {
                    'DatalistIdKey': 'Test2',
                    'DatalistAcKey': 'Test3'
                }
            }
        });

    equal($('#AutocompleteSelect').val(), 'Test2');
    equal($('#DatalistAutocompleteSelect').val(), 'Test3');
});

test('Autocomplete select prevented', 2, function () {
    $('#DatalistAutocompleteSelectPrevent').datalist({
        select: function (e, element, hiddenElement, data) {
            hiddenElement.value = 2;
            element.value = 11;
            e.preventDefault();
        }
    });

    $('#DatalistAutocompleteSelectPrevent').data('ui-autocomplete')
        ._trigger('select', 'autocompleteselect', {
            item: {
                label: 'Test0',
                value: 'Test1',
                item: {
                    'DatalistIdKey': 'Test2',
                    'DatalistAcKey': 'Test3'
                }
            }
        });

    equal($('#DatalistAutocompleteSelectPrevent').val(), 11);
    equal($('#AutocompleteSelectPrevent').val(), 2);
});

test('Binds key up on autocomplete', 6, function () {
    $('#AutocompleteKeyup').val('Test1');

    $('#DatalistAutocompleteKeyup').datalist({
        select: function (e, element, hiddenElement, data) {
            equal(element, $('#DatalistAutocompleteKeyup')[0]);
            equal(hiddenElement, $('#AutocompleteKeyup')[0]);
            equal(data, null);
            ok(e);
        }
    });

    $('#DatalistAutocompleteKeyup').keyup();
    equal($('#AutocompleteKeyup').val(), '');
    equal($('#DatalistAutocompleteKeyup').val(), '');
});

test('Bind key up on autocomplete prevents select', 2, function () {
    $('#AutocompleteKeyupPrevent').val('Test1');

    $('#DatalistAutocompleteKeyupPrevent').datalist({
        select: function (e, element, hiddenElement, data) {
            hiddenElement.value = 'Test2';
            element.value = 'Test3';
            e.preventDefault();
        }
    });

    $('#DatalistAutocompleteKeyupPrevent').keyup();
    equal($('#AutocompleteKeyupPrevent').val(), 'Test2');
    equal($('#DatalistAutocompleteKeyupPrevent').val(), 'Test3');
});

test('Removes all preceding elements', 1, function () {
    equal($('#DatalistAutocompleteRemoves').datalist().prevAll().length, 0);
});

asyncTest('Does not call select on load', 0, function () {
    $('#DatalistOnLoadNoSelect').datalist({
        select: function (e, element, hiddenElement, data) {
            ok(false);
        }
    });

    setTimeout(function () {
        start();
    }, 100);
});

asyncTest('Calls select on load', 1, function () {
    $('#DatalistOnLoadSelect').datalist({
        select: function (e, element, hiddenElement, data) {
            ok(true);
        }
    });

    setTimeout(function () {
        start();
    }, 100);
});

test('Cleans up datalist input', 7, function () {
    var datalistInput = $('#DatalistCleanUp').datalist();
    equal(datalistInput.attr('data-datalist-records-per-page'), null);
    equal(datalistInput.attr('data-datalist-dialog-title'), null);
    equal(datalistInput.attr('data-datalist-hidden-input'), null);
    equal(datalistInput.attr('data-datalist-sort-column'), null);
    equal(datalistInput.attr('data-datalist-sort-order'), null);
    equal(datalistInput.attr('data-datalist-filters'), null);
    equal(datalistInput.attr('data-datalist-url'), null);
});

test('Destroys datalist', function () {
    var datalistInput = $('#DatalistDestroy');
    datalistInput.attr('data-datalist-url', 'http://localhost:9140/Test');
    datalistInput.attr('data-datalist-dialog-title', 'TestTitle');
    datalistInput.attr('data-datalist-sort-column', 'TestColumn');
    datalistInput.attr('data-datalist-hidden-input', 'InitOptions');
    datalistInput.attr('data-datalist-records-per-page', 30);
    datalistInput.attr('data-datalist-sort-order', 'Desc');
    datalistInput.attr('data-datalist-filters', 'A,B,C');

    datalistInput.datalist();
    datalistInput.datalist('destroy');
    equal(datalistInput.attr('data-datalist-filters'), 'A,B,C');
    equal(datalistInput.attr('data-datalist-sort-order'), 'Desc');
    equal(datalistInput.attr('data-datalist-records-per-page'), 30);
    equal(datalistInput.attr('data-datalist-sort-column'), 'TestColumn');
    equal(datalistInput.attr('data-datalist-dialog-title'), 'TestTitle');
    equal(datalistInput.attr('data-datalist-hidden-input'), 'InitOptions');
    equal(datalistInput.attr('data-datalist-url'), 'http://localhost:9140/Test');
});