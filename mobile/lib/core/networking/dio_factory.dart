import 'package:dio/dio.dart';
import 'package:flutter/foundation.dart';
import 'package:pretty_dio_logger/pretty_dio_logger.dart';
import 'package:healthcare_app/core/storage/secure_storage_helper.dart';
import 'package:healthcare_app/core/storage/storage_keys.dart';

class DioFactory {
  DioFactory._();

  static Dio? _dio;

  static Future<Dio> getDio(SecureStorageHelper secureStorage) async {
    const timeOut = Duration(seconds: 30);

    if (_dio == null) {
      _dio = Dio()
        ..options.connectTimeout = timeOut
        ..options.receiveTimeout = timeOut
        ..options.headers = {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
        };

      await _addDioHeaders(secureStorage);
      _addDioInterceptors();
    }
    return _dio!;
  }

  static Future<void> _addDioHeaders(SecureStorageHelper secureStorage) async {
    final token = await secureStorage.getString(StorageKeys.userToken);
    if (token != null && token.isNotEmpty) {
      _dio?.options.headers['Authorization'] = 'Bearer $token';
    }
  }

  static void setTokenIntoHeaderAfterLogin(String token) {
    _dio?.options.headers['Authorization'] = 'Bearer $token';
  }

  static void removeTokenFromHeader() {
    _dio?.options.headers.remove('Authorization');
  }

  static void _addDioInterceptors() {
    if (kDebugMode) {
      _dio?.interceptors.add(
        PrettyDioLogger(
          requestBody: true,
          requestHeader: true,
          responseHeader: false,
          responseBody: true,
          error: true,
        ),
      );
    }
  }
}