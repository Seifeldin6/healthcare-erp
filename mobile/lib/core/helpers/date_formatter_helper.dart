class DateFormatterHelper {
  DateFormatterHelper._();

  static const List<String> _monthsEn = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December',
  ];

  static const List<String> _monthsAr = [
    'يناير',
    'فبراير',
    'مارس',
    'أبريل',
    'مايو',
    'يونيو',
    'يوليو',
    'أغسطس',
    'سبتمبر',
    'أكتوبر',
    'نوفمبر',
    'ديسمبر',
  ];

  /// Formats date as "1 January 1998" or "1 يناير 1998" for UI display.
  static String formatReadableDate(
    DateTime? date, {
    required bool isArabic,
    String fallback = '—',
  }) {
    if (date == null) return fallback;
    final monthName =
        isArabic ? _monthsAr[date.month - 1] : _monthsEn[date.month - 1];
    return '${date.day} $monthName ${date.year}';
  }

  /// Formats date as "DD / MM / YYYY" for input fields.
  static String formatNumericDate(DateTime date) {
    final day = date.day.toString().padLeft(2, '0');
    final month = date.month.toString().padLeft(2, '0');
    return '$day / $month / ${date.year}';
  }

  /// Formats date as ISO "YYYY-MM-DD" for backend API payloads.
  static String formatForApi(DateTime date) {
    final month = date.month.toString().padLeft(2, '0');
    final day = date.day.toString().padLeft(2, '0');
    return '${date.year}-$month-$day';
  }
}