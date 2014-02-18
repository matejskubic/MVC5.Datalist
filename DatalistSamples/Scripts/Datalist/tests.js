var testInput, hiddenInput, openSpan, filter1, filter2;

QUnit.testStart(function (details) {
    openSpan = $('#test-data > .input-group > .datalist-open-span');
    testInput = $('#TestDatalist');
    hiddenInput = $('#Test');
    filter1 = $('#Filter1');
    filter2 = $('#Filter2');

    testInput
        .attr('data-datalist-url', 'http://localhost:9140/Datalist/Default')
        .attr('data-datalist-filters', 'Filter1,Filter2')
        .attr('data-datalist-dialog-title', 'TestTitle')
        .attr('data-datalist-sort-column', 'FirstName')
        .attr('class', 'form-control datalist-input')
        .attr('data-datalist-records-per-page', 30)
        .attr('data-datalist-hidden-input', 'Test')
        .attr('data-datalist-sort-order', 'Desc')
        .attr('data-datalist-term', 'test')
        .attr('data-datalist-page', '1');
});
QUnit.testDone(function (details) {
    testInput.val('').clone().appendTo('#test-data > .input-group');
    testInput.remove();

    hiddenInput.val('').clone().appendTo('#test-data > .input-group');
    hiddenInput.remove();
    
    openSpan.clone().appendTo('#test-data > .input-group');
    openSpan.remove();
    
    filter1.val('').clone().appendTo('#test-data');
    filter1.remove();

    filter2.val('').clone().appendTo('#test-data');
    filter2.remove();
});

test('Init options', 12, function () {
    testInput.datalist();

    ok(testInput.hasClass('mvc-datalist'));
    equal(testInput.datalist('option', 'page'), 1);
    equal(testInput.datalist('option', 'term'), 'test');
    equal(testInput.datalist('option', 'select'), null);
    equal(testInput.datalist('option', 'sortOrder'), 'Desc');
    equal(testInput.datalist('option', 'title'), 'TestTitle');
    equal(testInput.datalist('option', 'filterChange'), null);
    equal(testInput.datalist('option', 'recordsPerPage'), 30);
    equal(testInput.datalist('option', 'sortColumn'), 'FirstName');
    equal(testInput.datalist('option', 'hiddenElement'), hiddenInput[0]);
    equal(testInput.datalist('option', 'url'), 'http://localhost:9140/Datalist/Default');
    equal(testInput.datalist('option', 'filters').join(), ['Filter1', 'Filter2'].join());
});
test('Init options limit records per page', 3, function () {
    testInput.attr('data-datalist-records-per-page', 'NaN').datalist();
    equal(testInput.datalist('option', 'recordsPerPage'), 20);

    testInput.datalist('destroy').attr('data-datalist-records-per-page', -1).datalist();
    equal(testInput.datalist('option', 'recordsPerPage'), 1);

    testInput.datalist('destroy').attr('data-datalist-records-per-page', 100).datalist();
    equal(testInput.datalist('option', 'recordsPerPage'), 99);
});

test('Default filters binding', 20, function () {
    var filters = [filter1, filter2];
    var iteration = 0;

    testInput.datalist({
        filterChange: function (e, element, hiddenElement, filter) {
            equal(filter, filters[iteration++][0]);
            equal(hiddenElement, hiddenInput[0]);
            equal(element, testInput[0]);
            ok(e);
        },
        select: function (e, element, hiddenElement, data) {
            equal(hiddenElement, hiddenInput[0]);
            equal(element, testInput[0]);
            equal(data, null);
            ok(e);
        }
    });
    
    testInput.val(1);
    hiddenInput.val(1);
    filters[0].change();
    equal(testInput.val(), '');
    equal(hiddenInput.val(), '');

    testInput.val(1);
    hiddenInput.val(1);
    filters[1].change();
    equal(testInput.val(), '');
    equal(hiddenInput.val(), '');
});
test('Custom filter for filter change', 2, function() {
    testInput.datalist({
        filterChange: function (e, element, hiddenElement, fitler) {
            hiddenElement.value = 'Test';
            element.value = 'Test';
            e.preventDefault();
        },
        select: function (e, element, hiddenElement, data) {
            ok(false);
        }
    });

    testInput.val(1);
    hiddenInput.val(1);
    filter1.change();
    equal(hiddenInput.val(), 'Test');
    equal(testInput.val(), 'Test');
});
test('Custom select for filter change', 2, function () {
    testInput.datalist({
        select: function (e, element, hiddenElement, data) {
            hiddenElement.value = 'Test';
            element.value = 'Test';
            e.preventDefault();
        }
    });

    testInput.val(1);
    hiddenInput.val(1);
    filter1.change();
    equal(testInput.val(), 'Test');
    equal(hiddenInput.val(), 'Test');
});
// TODO: Somehow test autocomplete source method
test('Creates autocomplete', 2, function () {
    ok(testInput.datalist().hasClass('ui-autocomplete-input'));
    equal(testInput.autocomplete('option', 'minLength'), 1);
});
test('Autocomplete select', 7, function () {
    testInput.datalist({
        select: function (e, element, hiddenElement, data) {
            equal(hiddenElement, hiddenInput[0]);
            equal(data.DatalistIdKey, 'Test2');
            equal(data.DatalistAcKey, 'Test3');
            equal(element, testInput[0]);
            ok(e);
        }
    });

    testInput.data('ui-autocomplete')
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
    equal(testInput.val(), 'Test3');
});
test('Autocomplete select prevented', 2, function () {
    testInput.datalist({
        select: function (e, element, hiddenElement, data) {
            hiddenElement.value = 22;
            element.value = 11;
            e.preventDefault();
        }
    });

    testInput.data('ui-autocomplete')
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

    equal(testInput.val(), 11);
    equal(hiddenInput.val(), 22);
});
test('Bind key up on autocomplete', 6, function () {
    testInput.datalist({
        select: function (e, element, hiddenElement, data) {
            equal(hiddenElement, hiddenInput[0]);
            equal(element, testInput[0]);
            equal(data, null);
            ok(e);
        }
    });

    hiddenInput.val('Test1');
    testInput.keyup();
    equal(testInput.val(), '');
    equal(hiddenInput.val(), '');
});
test('Bind key up on autocomplete select prevented', 2, function () {
    testInput.datalist({
        select: function (e, element, hiddenElement, data) {
            hiddenElement.value = 'Test2';
            element.value = 'Test3';
            e.preventDefault();
        }
    });

    testInput.keyup();
    equal(testInput.val(), 'Test3');
    equal(hiddenInput.val(), 'Test2');
});
test('Removes all preceding elements', 1, function () {
    equal(testInput.datalist().prevAll().length, 0);
});

test('Forms autocomplete url', 1, function () {
    testInput
        .attr('data-datalist-filters', '')
        .datalist();
    var expected = testInput.datalist('option', 'url') +
        '?SearchTerm=test' +
        '&RecordsPerPage=20' +
        '&SortOrder=Asc' +
        '&Page=0';

    equal(testInput.data('mvc-datalist')._formAutocompleteUrl('test'), expected);
});
test('Forms autocomplete url with filters', 1, function () {
    testInput.datalist();

    filter1.val('Filter1');
    filter2.val('Filter2');
    var expected = testInput.datalist('option', 'url') +
        '?SearchTerm=test' +
        '&RecordsPerPage=20' +
        '&SortOrder=Asc' +
        '&Page=0' +
        '&Filter1=Filter1' +
        '&Filter2=Filter2';

    equal(testInput.data('mvc-datalist')._formAutocompleteUrl('test'), expected);
});

test('Forms datalist url', 1, function () {
    testInput
        .attr('data-datalist-filters', '')
        .datalist();
    var expected = testInput.datalist('option', 'url') +
        '?SearchTerm=test' +
        '&RecordsPerPage=' + testInput.datalist('option', 'recordsPerPage') +
        '&SortColumn=' + testInput.datalist('option', 'sortColumn') +
        '&SortOrder=' + testInput.datalist('option', 'sortOrder') +
        '&Page=' + testInput.datalist('option', 'page');

    equal(testInput.data('mvc-datalist')._formDatalistUrl('test'), expected);
});
test('Forms datalist url with filters', 1, function () {
    testInput.datalist();

    filter1.val('Filter1');
    filter2.val('Filter2');
    var expected = testInput.datalist('option', 'url') +
        '?SearchTerm=test' +
        '&RecordsPerPage=' + testInput.datalist('option', 'recordsPerPage') +
        '&SortColumn=' + testInput.datalist('option', 'sortColumn') +
        '&SortOrder=' + testInput.datalist('option', 'sortOrder') +
        '&Page=' + testInput.datalist('option', 'page') +
        '&Filter1=Filter1' +
        '&Filter2=Filter2';

    equal(testInput.data('mvc-datalist')._formDatalistUrl('test'), expected);
});

test('Forms empty additional filter query', 1, function () {
    testInput.attr('data-datalist-filters', '').datalist();
    equal(testInput.data('mvc-datalist')._formAdditionalFiltersQuery(), '');
});
test('Forms additional filter query', 1, function () {
    testInput.datalist();
    filter1.val('Test1');
    filter2.val('Test2');
    equal(testInput.data('mvc-datalist')._formAdditionalFiltersQuery(), '&Filter1=Test1&Filter2=Test2');
});

asyncTest('Does not call select on load', 0, function () {
    testInput.datalist({
        select: function (e, element, hiddenElement, data) {
            ok(false);
        }
    });

    setTimeout(function () {
        start();
    }, 200);
});
asyncTest('Calls select on load', 5, function () {
    hiddenInput.val(1);
    testInput.datalist({
        select: function (e, element, hiddenElement, data) {
            equal(element, testInput[0]);
            equal(hiddenElement, hiddenInput[0]);
            equal(data.DatalistAcKey, 'Tom');
            equal(data.DatalistIdKey, '1');
            ok(e);
        }
    });

    setTimeout(function () {
        start();
    }, 200);
});
asyncTest('Select on load prevented', 2, function () {
    hiddenInput.val(1);
    testInput.datalist({
        select: function (e, element, hiddenElement, data) {
            hiddenElement.value = 'Test1';
            element.value = 'Test2';
            e.preventDefault();
        }
    });

    setTimeout(function () {
        start();
        equal(testInput.val(), 'Test2');
        equal(hiddenInput.val(), 'Test1');
    }, 200);
});

test('Binds datalist', 7, function () {
    testInput.datalist();
    openSpan.click();
    $('#Datalist').dialog('close');
    
    $('.datalist-items-per-page').val(2).data('ui-spinner')._trigger('change', 'spinnerchange');
    equal($('#Datalist').dialog('option', 'title'), testInput.datalist('option', 'title'));
    equal($('.datalist-search-input').attr('placeholder'), $.fn.datalist.lang.Search);
    equal($('.datalist-error-span').html(), $.fn.datalist.lang.Error);
    equal(testInput.datalist('option', 'recordsPerPage'), 2);
    equal(testInput.datalist('option', 'page'), 0);

    $('.datalist-search-input').val('test2').keyup();
    stop();
    setTimeout(function () {
        start();
        equal(testInput.datalist('option', 'term'), 'test2');
        equal(testInput.datalist('option', 'page'), 0);
    }, 500);
});

test('Limits value', 6, function () {
    testInput.datalist();
    var datalist = testInput.data('mvc-datalist');

    equal(datalist._limitTo('NotNumber', 1, 99), 20);
    equal(datalist._limitTo(-1, 1, 99), 1);
    equal(datalist._limitTo(1, 1, 99), 1);
    equal(datalist._limitTo(100, 1, 99), 99);
    equal(datalist._limitTo(99, 1, 99), 99);
    equal(datalist._limitTo(60, 1, 99), 60);
});

test('Cleans up datalist input', 18, function () {
    testInput.datalist();
    equal(testInput.attr('data-datalist-records-per-page'), null);
    equal(testInput.attr('data-datalist-dialog-title'), null);
    equal(testInput.attr('data-datalist-hidden-input'), null);
    equal(testInput.attr('data-datalist-sort-column'), null);
    equal(testInput.attr('data-datalist-sort-order'), null);
    equal(testInput.attr('data-datalist-filters'), null);
    equal(testInput.attr('data-datalist-term'), null);
    equal(testInput.attr('data-datalist-page'), null);
    equal(testInput.attr('data-datalist-url'), null);

    testInput.datalist('destroy').datalist();
    equal(testInput.attr('data-datalist-records-per-page'), null);
    equal(testInput.attr('data-datalist-dialog-title'), null);
    equal(testInput.attr('data-datalist-hidden-input'), null);
    equal(testInput.attr('data-datalist-sort-column'), null);
    equal(testInput.attr('data-datalist-sort-order'), null);
    equal(testInput.attr('data-datalist-filters'), null);
    equal(testInput.attr('data-datalist-term'), null);
    equal(testInput.attr('data-datalist-page'), null);
    equal(testInput.attr('data-datalist-url'), null);
});

test('Destroys datalist', 9, function () {
    testInput.datalist({
        filterChange: function () {
            ok(false);
        }
    });

    testInput.datalist('destroy');
    equal(testInput.attr('data-datalist-sort-order'), 'Desc');
    equal(testInput.attr('data-datalist-hidden-input'), 'Test');
    equal(testInput.attr('data-datalist-records-per-page'), 30);
    equal(testInput.attr('data-datalist-sort-column'), 'FirstName');
    equal(testInput.attr('data-datalist-dialog-title'), 'TestTitle');
    equal(testInput.attr('data-datalist-filters'), 'Filter1,Filter2');
    equal(testInput.attr('data-datalist-url'), 'http://localhost:9140/Datalist/Default');

    equal(testInput.hasClass('ui-autocomplete-input'), false);
    equal(testInput.hasClass('mvc-datalist'), false);
    filter1.change();
    filter2.change();
});

test('Datalist language init', 3, function () {
    equal($.fn.datalist.lang.Error, 'Error while retrieving records');
    equal($.fn.datalist.lang.NoDataFound, 'No data found');
    equal($.fn.datalist.lang.Search, 'Search...');
});
test('Datalist spinner init', 4, function () {
    var datalistSpinner = $('.datalist-items-per-page');
    ok(datalistSpinner.hasClass('ui-spinner-input'));
    equal(datalistSpinner.spinner('option', 'min'), 1);
    equal(datalistSpinner.spinner('option', 'max'), 99);
    ok(datalistSpinner.parent().hasClass('input-group-addon'));
});
test('Datalist dialog init', 7, function () {
    var datalist = $('#Datalist');
    ok(datalist.hasClass('ui-dialog-content'));
    equal(datalist.dialog('option', 'autoOpen'), false);
    equal(datalist.dialog('option', 'minHeight'), 210);
    equal(datalist.dialog('option', 'height'), 'auto');
    equal(datalist.dialog('option', 'minWidth'), 455);
    equal(datalist.dialog('option', 'width'), 'auto');
    equal(datalist.dialog('option', 'modal'), true);
});