import 'package:healthcare_app/core/networking/api_result.dart';
import 'package:healthcare_app/features/auth/data/models/register_patient_request_model.dart';
import 'package:healthcare_app/features/auth/domain/entities/login_entity.dart';
import 'package:healthcare_app/features/auth/domain/repositories/auth_repo.dart';

class RegisterPatientUseCase {
  final AuthRepo _authRepo;

  RegisterPatientUseCase(this._authRepo);

  Future<ApiResult<LoginEntity>> call(
    RegisterPatientRequestModel requestModel,
  ) {
    return _authRepo.registerPatient(requestModel);
  }
}