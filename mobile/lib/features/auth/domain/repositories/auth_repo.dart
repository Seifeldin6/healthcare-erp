import 'package:healthcare_app/core/networking/api_result.dart';
import 'package:healthcare_app/features/auth/domain/entities/login_entity.dart';

abstract class AuthRepo {
  Future<ApiResult<LoginEntity>> login({
    required String emailOrPhone,
    required String password,
  });
}