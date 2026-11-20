import 'package:flutter/foundation.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class SecureStorageHelper {
  final FlutterSecureStorage _storage;

  SecureStorageHelper(this._storage);

  Future<void> setString(String key, String value) async {
    debugPrint('SecureStorageHelper : setString [$key]');
    await _storage.write(key: key, value: value);
  }

  Future<String?> getString(String key) async {
    final value = await _storage.read(key: key);
    debugPrint('SecureStorageHelper : getString [$key] = ${value != null ? '***' : 'null'}');
    return value;
  }

  Future<void> remove(String key) async {
    debugPrint('SecureStorageHelper : remove [$key]');
    await _storage.delete(key: key);
  }

  Future<void> clearAll() async {
    debugPrint('SecureStorageHelper : clearAll');
    await _storage.deleteAll();
  }
}