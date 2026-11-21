import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:healthcare_app/core/networking/api_result.dart';
import 'package:healthcare_app/features/auth/data/models/register_patient_request_model.dart';
import 'package:healthcare_app/features/auth/domain/usecases/register_patient_use_case.dart';
import 'package:healthcare_app/features/auth/presentation/cubit/auth_state.dart';

class RegisterPatientCubit extends Cubit<AuthState> {
  final RegisterPatientUseCase _registerPatientUseCase;

  RegisterPatientCubit(this._registerPatientUseCase)
      : super(const AuthState.initial());

  Future<void> registerPatient(
    RegisterPatientRequestModel requestModel,
  ) async {
    emit(const AuthState.loading());

    final result = await _registerPatientUseCase(requestModel);

    switch (result) {
      case Success(:final data):
        emit(AuthState.success(data));
      case Failure(:final errorHandler):
        emit(AuthState.failure(errorHandler.message));
    }
  }
}