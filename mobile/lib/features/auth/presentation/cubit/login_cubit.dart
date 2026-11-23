import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:healthcare_app/core/networking/api_result.dart';
import 'package:healthcare_app/features/auth/domain/usecases/login_use_case.dart';
import 'package:healthcare_app/features/auth/presentation/cubit/auth_state.dart';

class LoginCubit extends Cubit<AuthState> {
  final LoginUseCase _loginUseCase;

  LoginCubit(this._loginUseCase) : super(const AuthState.initial());

  Future<void> login({
    required String emailOrPhone,
    required String password,
  }) async {
    emit(const AuthState.loading());

    final result = await _loginUseCase(
      emailOrPhone: emailOrPhone.trim(),
      password: password,
    );

    switch (result) {
      case Success(:final data):
        emit(AuthState.success(data));
      case Failure(:final errorHandler):
        emit(AuthState.failure(errorHandler.message));
    }
  }
}