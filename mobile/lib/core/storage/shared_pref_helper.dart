import 'package:flutter/foundation.dart';
import 'package:shared_preferences/shared_preferences.dart';

class SharedPrefHelper {
  final SharedPreferences _prefs;

  SharedPrefHelper(this._prefs);

  Future<bool> setString(String key, String value) async {
    debugPrint('SharedPrefHelper : setString [$key] = $value');
    return _prefs.setString(key, value);
  }

  String? getString(String key) {
    final value = _prefs.getString(key);
    debugPrint('SharedPrefHelper : getString [$key] = $value');
    return value;
  }

  Future<bool> setBool(String key, bool value) async {
    debugPrint('SharedPrefHelper : setBool [$key] = $value');
    return _prefs.setBool(key, value);
  }

  bool getBool(String key, {bool defaultValue = false}) {
    final value = _prefs.getBool(key) ?? defaultValue;
    debugPrint('SharedPrefHelper : getBool [$key] = $value');
    return value;
  }

  Future<bool> setInt(String key, int value) async {
    debugPrint('SharedPrefHelper : setInt [$key] = $value');
    return _prefs.setInt(key, value);
  }

  int getInt(String key, {int defaultValue = 0}) {
    final value = _prefs.getInt(key) ?? defaultValue;
    debugPrint('SharedPrefHelper : getInt [$key] = $value');
    return value;
  }

  Future<bool> remove(String key) async {
    debugPrint('SharedPrefHelper : remove [$key]');
    return _prefs.remove(key);
  }

  Future<bool> clearAll() async {
    debugPrint('SharedPrefHelper : clearAll');
    return _prefs.clear();
  }
}