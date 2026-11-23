class BloodTypeHelper {
  BloodTypeHelper._();

  static const String unknown = 'Unknown';
  static const List<String> firstRowTypes = ['A+', 'A-', 'B+', 'B-'];
  static const List<String> secondRowTypes = ['AB+', 'AB-', 'O+', 'O-'];

  static const Map<String, String> _uiToApiMap = {
    'A+': 'APositive',
    'A-': 'ANegative',
    'B+': 'BPositive',
    'B-': 'BNegative',
    'AB+': 'ABPositive',
    'AB-': 'ABNegative',
    'O+': 'OPositive',
    'O-': 'ONegative',
  };

  static final Map<String, String> _apiToUiMap = {
    for (final entry in _uiToApiMap.entries) entry.value: entry.key,
  };

  /// Converts UI display value (e.g. 'A+') to API enum string (e.g. 'APositive').
  static String? toApiValue(String? uiValue) {
    if (uiValue == null || uiValue == unknown) return null;
    return _uiToApiMap[uiValue];
  }

  /// Converts API enum string (e.g. 'APositive') back to UI display value (e.g. 'A+').
  static String? toUiValue(String? apiValue) {
    if (apiValue == null) return null;
    return _apiToUiMap[apiValue] ?? apiValue;
  }
}