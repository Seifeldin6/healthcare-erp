import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:healthcare_app/core/networking/api_result.dart';
import 'package:healthcare_app/features/auth/domain/usecases/login_use_case.dart';
import 'package:healthcare_app/features/auth/presentation/cubit/login_state.dart';

class LoginCubit extends Cubit<LoginState> {
  final LoginUseCase _loginUseCase;

  LoginCubit(this._loginUseCase) : super(const LoginState.initial());

  Future<void> login({
    required String emailOrPhone,
    required String password,
  }) async {
    emit(const LoginState.loading());

    final result = await _loginUseCase(
      emailOrPhone: emailOrPhone.trim(),
      password: password,
    );

    switch (result) {
      case Success(:final data):
        emit(LoginState.success(data));
      case Failure(:final errorHandler):
        emit(LoginState.failure(errorHandler.message));
    }
  }
}