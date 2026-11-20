import 'package:dio/dio.dart';
import 'package:healthcare_app/core/helpers/extensions.dart';
import 'package:healthcare_app/core/networking/api_error_model.dart';

class ApiErrorHandler {
  final String message;
  final int? status;

  const ApiErrorHandler._(this.message, this.status);

  static ApiErrorHandler handle(Object error) {
    if (error is DioException) {
      final statusCode = error.response?.statusCode;

      switch (error.type) {
        case DioExceptionType.connectionTimeout:
        case DioExceptionType.sendTimeout:
        case DioExceptionType.receiveTimeout:
          return ApiErrorHandler._(
            'Connection timed out. Please try again.',
            statusCode,
          );

        case DioExceptionType.connectionError:
          return ApiErrorHandler._(
            'No internet connection. Please check your network.',
            statusCode,
          );

        case DioExceptionType.badCertificate:
          return ApiErrorHandler._('Security certificate error.', statusCode);

        case DioExceptionType.badResponse:
          return _handleBadResponse(error.response, statusCode);

        case DioExceptionType.cancel:
          return ApiErrorHandler._('Request was cancelled.', statusCode);

        case DioExceptionType.unknown:
        case DioExceptionType.transformTimeout:
          return ApiErrorHandler._(
            'Something went wrong. Please try again.',
            statusCode,
          );
      }
    }

    return const ApiErrorHandler._('An unexpected error occurred.', null);
  }

  static ApiErrorHandler _handleBadResponse(
    Response? response,
    int? statusCode,
  ) {
    final data = response?.data;

    if (data is Map<String, dynamic>) {
      final apiErrorModel = ApiErrorModel.fromJson(data);
      if (!(apiErrorModel.message.isNullOrEmpty())) {
        return ApiErrorHandler._(apiErrorModel.message!, statusCode);
      }
    }

    switch (statusCode) {
      case 401:
        return ApiErrorHandler._(
          'Invalid email/phone or password. Please try again.',
          statusCode,
        );
      case 403:
        return ApiErrorHandler._(
          'Access denied. You do not have permission to perform this action.',
          statusCode,
        );
      default:
        return ApiErrorHandler._(
          'Server error ($statusCode). Please try again.',
          statusCode,
        );
    }
  }
}
