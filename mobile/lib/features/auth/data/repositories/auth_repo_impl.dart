import 'package:healthcare_app/core/networking/api_error_handler.dart';
import 'package:healthcare_app/core/networking/api_result.dart';
import 'package:healthcare_app/core/networking/dio_factory.dart';
import 'package:healthcare_app/core/storage/secure_storage_helper.dart';
import 'package:healthcare_app/core/storage/storage_keys.dart';
import 'package:healthcare_app/features/auth/data/datasources/auth_remote_data_source.dart';
import 'package:healthcare_app/features/auth/data/models/login_request_model.dart';
import 'package:healthcare_app/features/auth/data/models/login_response_model.dart';
import 'package:healthcare_app/features/auth/domain/entities/login_entity.dart';
import 'package:healthcare_app/features/auth/domain/repositories/auth_repo.dart';

class AuthRepoImpl implements AuthRepo {
  final AuthRemoteDataSource _remoteDataSource;
  final SecureStorageHelper _secureStorage;

  AuthRepoImpl(this._remoteDataSource, this._secureStorage);

  @override
  Future<ApiResult<LoginEntity>> login({
    required String emailOrPhone,
    required String password,
  }) async {
    try {
      final response = await _remoteDataSource.login(
        LoginRequestModel(emailOrPhone: emailOrPhone, password: password),
      );
      await _saveUserSession(response);
      return ApiResult.success(response);
    } catch (error) {
      return ApiResult.failure(ApiErrorHandler.handle(error));
    }
  }

  Future<void> _saveUserSession(LoginResponseModel response) async {
    await Future.wait([
      _secureStorage.setString(StorageKeys.userToken, response.token),
      _secureStorage.setString(StorageKeys.userId, response.userId),
      _secureStorage.setString(StorageKeys.userRole, response.role),
      if (response.tenantId != null)
        _secureStorage.setString(StorageKeys.tenantId, response.tenantId!),
      if (response.patientId != null)
        _secureStorage.setString(StorageKeys.patientId, response.patientId!),
    ]);
    DioFactory.setTokenIntoHeaderAfterLogin(response.token);
  }
}
