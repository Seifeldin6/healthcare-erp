import 'package:healthcare_app/core/networking/api_result.dart';
import 'package:healthcare_app/features/auth/domain/entities/login_entity.dart';
import 'package:healthcare_app/features/auth/domain/repositories/auth_repo.dart';

class LoginUseCase {
  final AuthRepo _authRepo;

  LoginUseCase(this._authRepo);

  Future<ApiResult<LoginEntity>> call({
    required String emailOrPhone,
    required String password,
  }) {
    return _authRepo.login(
      emailOrPhone: emailOrPhone,
      password: password,
    );
  }
}