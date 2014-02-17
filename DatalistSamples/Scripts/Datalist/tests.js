test('Init options', 11, function() {
    var input = $('#InitOptionsDatalist')
        .attr('data-datalist-url', 'http://localhost:9140/Test')
        .attr('data-datalist-hidden-input', 'InitOptions')
        .attr('data-datalist-dialog-title', 'TestTitle')
        .attr('data-datalist-sort-column', 'TestColumn')
        .attr('data-datalist-records-per-page', 30)
        .attr('data-datalist-sort-order', 'Desc')
        .attr('data-datalist-filters', 'A,B,C')
        .datalist();

    equal(input.datalist('option', 'page'), 0);
    equal(input.datalist('option', 'term'), '');
    equal(input.datalist('option', 'select'), null);
    equal(input.datalist('option', 'sortOrder'), 'Desc');
    equal(input.datalist('option', 'title'), 'TestTitle');
    equal(input.datalist('option', 'filterChange'), null);
    equal(input.datalist('option', 'recordsPerPage'), 30);
    equal(input.datalist('option', 'sortColumn'), 'TestColumn');
    equal(input.datalist('option', 'url'), 'http://localhost:9140/Test');
    equal(input.datalist('option', 'filters').join(), ['A', 'B', 'C'].join());
    equal(input.datalist('option', 'hiddenElement'), $('#InitOptions')[0]);
});
test('Init options limit records per page', 3, function () {
    var input = $('#InitOptionsLimitDatalist');
    input.attr('data-datalist-records-per-page', 'NaN').datalist();
    equal(input.datalist('option', 'recordsPerPage'), 20);

    input.datalist('destroy').attr('data-datalist-records-per-page', -1).datalist();
    equal(input.datalist('option', 'recordsPerPage'), 1);

    input.datalist('destroy').attr('data-datalist-records-per-page', 100).datalist();
    equal(input.datalist('option', 'recordsPerPage'), 99);
});

test('Default filters binding', 20, function () {
    var input = $('#DefaultFilterDatalist').attr('data-datalist-filters', 'Filter1,Filter2');
    var filters = [$('#Filter1'), $('#Filter2')];
    var hiddenInput = $('#DefaultFilter');
    var iteration = 0;

    input.datalist({
        filterChange: function (e, element, hiddenElement, filter) {
            equal(filter, filters[iteration++][0]);
            equal(hiddenElement, hiddenInput[0]);
            equal(element, input[0]);
            ok(e);
        },
        select: function (e, element, hiddenElement, data) {
            equal(hiddenElement, hiddenInput[0]);
            equal(element, input[0]);
            equal(data, null);
            ok(e);
        }
    });
    
    input.val(1);
    hiddenInput.val(1);
    filters[0].change();
    equal(input.val(), '');
    equal(hiddenInput.val(), '');

    input.val(1);
    hiddenInput.val(1);
    filters[1].change();
    equal(input.val(), '');
    equal(hiddenInput.val(), '');
});
test('Custom filter for filter change', 2, function() {
    var hiddenInput = $('#CustomFilterChange');
    var input = $('#CustomFilterChangeDatalist')
        .attr('data-datalist-filters', 'CustomFilter')
        .datalist({
            filterChange: function (e, element, hiddenElement, fitler) {
                hiddenElement.value = 'Test';
                element.value = 'Test';
                e.preventDefault();
            },
            select: function (e, element, hiddenElement, data) {
                ok(false);
            }
        });

    input.val(1);
    hiddenInput.val(1);
    $('#CustomFilter').change();
    equal(hiddenInput.val(), 'Test');
    equal(input.val(), 'Test');
});
test('Custom select for filter change', 2, function () {
    var hiddenInput = $('#CustomSelectFilterChange');
    var input = $('#CustomSelectFilterChangeDatalist')
        .attr('data-datalist-filters', 'CustomSelectFilter')
        .datalist({
            select: function (e, element, hiddenElement, data) {
                hiddenElement.value = 'Test';
                element.value = 'Test';
                e.preventDefault();
            }
        });

    input.val(1);
    hiddenInput.val(1);
    $('#CustomSelectFilter').change();

    equal(input.val(), 'Test');
    equal(hiddenInput.val(), 'Test');
});
// TODO: Somehow test autocomplete source method
test('Creates autocomplete', 2, function () {
    ok($('#AutocompleteDatalist').datalist().hasClass('ui-autocomplete-input'));
    equal($('#AutocompleteDatalist').autocomplete('option', 'minLength'), 1)
});
test('Autocomplete select', 7, function () {
    var hiddenInput = $('#AutocompleteSelect');
    var input = $('#AutocompleteSelectDatalist').datalist({
        select: function (e, element, hiddenElement, data) {
            equal(hiddenElement, hiddenInput[0]);
            equal(data.DatalistIdKey, 'Test2');
            equal(data.DatalistAcKey, 'Test3');
            equal(element, input[0]);
            ok(e);
        }
    });

    input.data('ui-autocomplete')
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

    equal(hiddenInput.val(), 'Test2');
    equal(input.val(), 'Test3');
});
test('Autocomplete select prevented', 2, function () {
    var input = $('#AutocompleteSelectPreventDatalist').datalist({
        select: function (e, element, hiddenElement, data) {
            hiddenElement.value = 22;
            element.value = 11;
            e.preventDefault();
        }
    });

    input.data('ui-autocomplete')
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

    equal(input.val(), 11);
    equal($('#AutocompleteSelectPrevent').val(), 22);
});
test('Bind key up on autocomplete', 6, function () {
    var hiddenInput = $('#AutocompleteKeyup');
    var input = $('#AutocompleteKeyupDatalist').datalist({
        select: function (e, element, hiddenElement, data) {
            equal(hiddenElement, hiddenInput[0]);
            equal(element, input[0]);
            equal(data, null);
            ok(e);
        }
    });

    hiddenInput.val('Test1');
    input.keyup();
    equal(input.val(), '');
    equal(hiddenInput.val(), '');
});
test('Bind key up on autocomplete select prevented', 2, function () {
    var hiddenInput = $('#AutocompleteKeyupPrevent');
    var input = $('#AutocompleteKeyupPreventDatalist').datalist({
        select: function (e, element, hiddenElement, data) {
            hiddenElement.value = 'Test2';
            element.value = 'Test3';
            e.preventDefault();
        }
    });

    input.keyup();
    equal(input.val(), 'Test3');
    equal(hiddenInput.val(), 'Test2');
});
test('Removes all preceding elements', 1, function () {
    equal($('#AutocompleteRemoveDatalist').datalist().prevAll().length, 0);
});

test('Forms autocomplete url', 1, function () {
    var input= $('#AutocompleteUrlDatalist').datalist();
    var expected = input.datalist('option', 'url') +
        '?SearchTerm=test' +
        '&RecordsPerPage=20' +
        '&SortOrder=Asc' +
        '&Page=0';

    equal(input.data('mvc-datalist')._formAutocompleteUrl('test'), expected);
});
test('Forms autocomplete url with filters', 1, function () {
    var input = $('#AutocompleteUrlWithFiltersDatalist')
        .attr('data-datalist-filters', 'AutocompleteUrlFilter1,AutocompleteUrlFilter2')
        .datalist();

    $('#AutocompleteUrlFilter1').val('Filter1');
    $('#AutocompleteUrlFilter2').val('Filter2');
    var expected = input.datalist('option', 'url') +
        '?SearchTerm=test' +
        '&RecordsPerPage=20' +
        '&SortOrder=Asc' +
        '&Page=0' +
        '&AutocompleteUrlFilter1=Filter1' +
        '&AutocompleteUrlFilter2=Filter2';

    equal(input.data('mvc-datalist')._formAutocompleteUrl('test'), expected);
});

test('Forms datalist url', 1, function () {
    var input = $('#DatalistUrlDatalist').datalist();
    var expected = input.datalist('option', 'url') +
        '?SearchTerm=test' +
        '&RecordsPerPage=' + input.datalist('option', 'recordsPerPage') +
        '&SortColumn=' + input.datalist('option', 'sortColumn') +
        '&SortOrder=' + input.datalist('option', 'sortOrder') +
        '&Page=' + input.datalist('option', 'page');

    equal(input.data('mvc-datalist')._formDatalistUrl('test'), expected);
});
test('Forms datalist url with filters', 1, function () {
    var input = $('#DatalistUrlWithFiltersDatalist')
        .attr('data-datalist-filters', 'DatalistUrlFilter1,DatalistUrlFilter2')
        .datalist();

    $('#DatalistUrlFilter1').val('Filter1');
    $('#DatalistUrlFilter2').val('Filter2');
    var expected = input.datalist('option', 'url') +
        '?SearchTerm=test' +
        '&RecordsPerPage=' + input.datalist('option', 'recordsPerPage') +
        '&SortColumn=' + input.datalist('option', 'sortColumn') +
        '&SortOrder=' + input.datalist('option', 'sortOrder') +
        '&Page=' + input.datalist('option', 'page') +
        '&DatalistUrlFilter1=Filter1' +
        '&DatalistUrlFilter2=Filter2';

    equal(input.data('mvc-datalist')._formDatalistUrl('test'), expected);
});

test('Forms empty additional filter query', 1, function () {
    var input = $('#FormEmptyAdditionalFilterDatalist').datalist();
    equal(input.data('mvc-datalist')._formAdditionalFiltersQuery(), '');
});
test('Forms additional filter query', 1, function () {
    var input = $('#FormAdditionalFilterDatalist')
        .attr('data-datalist-filters', 'FormAdditionalFilter1,FormAdditionalFilter2')
        .datalist();

    $('#FormAdditionalFilter1').val('Test1');
    $('#FormAdditionalFilter2').val('Test2');
    equal(input.data('mvc-datalist')._formAdditionalFiltersQuery(), '&FormAdditionalFilter1=Test1&FormAdditionalFilter2=Test2');
});

asyncTest('Does not call select on load', 0, function () {
    $('#OnLoadNoSelectDatalist').datalist({
        select: function (e, element, hiddenElement, data) {
            ok(false);
        }
    });

    setTimeout(function () {
        start();
    }, 100);
});
asyncTest('Calls select on load', 5, function () {
    $('#OnLoadSelectDatalist').datalist({
        select: function (e, element, hiddenElement, data) {
            equal(element, $('#OnLoadSelectDatalist')[0]);
            equal(hiddenElement, $('#OnLoadSelect')[0]);
            equal(data.DatalistAcKey, 'Tom');
            equal(data.DatalistIdKey, '1');
            ok(e);
        }
    });

    setTimeout(function () {
        start();
    }, 100);
});
asyncTest('Select on load prevented', 2, function () {
    var hiddenInput = $('#OnLoadSelectPrevented');
    var input = $('#OnLoadSelectPreventedDatalist').datalist({
        select: function (e, element, hiddenElement, data) {
            hiddenElement.value = 'Test1';
            element.value = 'Test2';
            e.preventDefault();
        }
    });

    setTimeout(function () {
        start();
        equal(input.val(), 'Test2');
        equal(hiddenInput.val(), 'Test1');
    }, 100);
});

test('Cleans up datalist input', 7, function () {
    var input = $('#CleanUpDatalist').datalist();
    equal(input.attr('data-datalist-records-per-page'), null);
    equal(input.attr('data-datalist-dialog-title'), null);
    equal(input.attr('data-datalist-hidden-input'), null);
    equal(input.attr('data-datalist-sort-column'), null);
    equal(input.attr('data-datalist-sort-order'), null);
    equal(input.attr('data-datalist-filters'), null);
    equal(input.attr('data-datalist-url'), null);
});

test('Destroys datalist', function () {
    var input = $('#DestroyDatalist')
        .attr('data-datalist-url', 'http://localhost:9140/Test')
        .attr('data-datalist-hidden-input', 'InitOptions')
        .attr('data-datalist-dialog-title', 'TestTitle')
        .attr('data-datalist-sort-column', 'TestColumn')
        .attr('data-datalist-records-per-page', 30)
        .attr('data-datalist-sort-order', 'Desc')
        .attr('data-datalist-filters', 'A,B,C');

    input.datalist();
    input.datalist('destroy');
    equal(input.attr('data-datalist-filters'), 'A,B,C');
    equal(input.attr('data-datalist-sort-order'), 'Desc');
    equal(input.attr('data-datalist-records-per-page'), 30);
    equal(input.attr('data-datalist-sort-column'), 'TestColumn');
    equal(input.attr('data-datalist-dialog-title'), 'TestTitle');
    equal(input.attr('data-datalist-hidden-input'), 'InitOptions');
    equal(input.attr('data-datalist-url'), 'http://localhost:9140/Test');
});