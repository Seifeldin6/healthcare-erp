import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:healthcare_app/features/auth/domain/entities/login_entity.dart';

part 'login_state.freezed.dart';

@freezed
sealed class LoginState with _$LoginState {
  const factory LoginState.initial() = _Initial;
  const factory LoginState.loading() = LoginLoading;
  const factory LoginState.success(LoginEntity loginEntity) = LoginSuccess;
  const factory LoginState.failure(String message) = LoginFailure;
}