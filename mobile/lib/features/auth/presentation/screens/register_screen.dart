import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/helpers/blood_type_helper.dart';
import 'package:healthcare_app/core/helpers/date_formatter_helper.dart';
import 'package:healthcare_app/core/helpers/extensions.dart';
import 'package:healthcare_app/core/routing/routes.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/features/auth/data/models/register_patient_request_model.dart';
import 'package:healthcare_app/features/auth/presentation/cubit/auth_state.dart';
import 'package:healthcare_app/features/auth/presentation/cubit/register_patient_cubit.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/register_header.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/register_navigation_buttons.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/register_step_four_emergency.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/register_step_one_account.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/register_step_three_clinical.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/register_step_two_personal.dart';
import 'package:healthcare_app/generated/l10n.dart';

class RegisterScreen extends StatefulWidget {
  const RegisterScreen({super.key});

  @override
  State<RegisterScreen> createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  final PageController _pageController = PageController();

  // Account Controllers
  final _emailController = TextEditingController();
  final _phoneController = TextEditingController();
  final _passwordController = TextEditingController();
  final _confirmPasswordController = TextEditingController();

  // Personal Controllers & State
  final _fullNameController = TextEditingController();
  final _nationalIdController = TextEditingController();
  DateTime? _selectedDateOfBirth;
  String? _selectedGender;

  // Clinical Controllers & State
  final _heightController = TextEditingController();
  final _weightController = TextEditingController();
  String? _selectedBloodType;

  // Emergency Contact Controllers
  final _emergencyNameController = TextEditingController();
  final _emergencyPhoneController = TextEditingController();

  // FormKeys
  final _step1FormKey = GlobalKey<FormState>();
  final _step2FormKey = GlobalKey<FormState>();
  final _step3FormKey = GlobalKey<FormState>();
  final _step4FormKey = GlobalKey<FormState>();

  int _currentIndex = 0;
  String? _serverEmailError;
  String? _serverPhoneError;
  String? _serverNationalIdError;
  bool _returnToReviewAfterStep1 = false;
  bool _returnToReviewAfterStep2 = false;

  @override
  void dispose() {
    _pageController.dispose();
    _emailController.dispose();
    _phoneController.dispose();
    _passwordController.dispose();
    _confirmPasswordController.dispose();
    _fullNameController.dispose();
    _nationalIdController.dispose();
    _heightController.dispose();
    _weightController.dispose();
    _emergencyNameController.dispose();
    _emergencyPhoneController.dispose();
    super.dispose();
  }

  void _nextPage() {
    _pageController.nextPage(
      duration: const Duration(milliseconds: 300),
      curve: Curves.easeInOut,
    );
  }

  void _previousPage() {
    if (_currentIndex > 0) {
      _pageController.previousPage(
        duration: const Duration(milliseconds: 300),
        curve: Curves.easeInOut,
      );
    }
  }

  void _goToPage(int index) {
    if (index == 0) {
      setState(() => _returnToReviewAfterStep1 = true);
    } else if (index == 1) {
      setState(() => _returnToReviewAfterStep2 = true);
    }
    _pageController.animateToPage(
      index,
      duration: const Duration(milliseconds: 300),
      curve: Curves.easeInOut,
    );
  }

  void _handleStep1Continue() {
    if (_returnToReviewAfterStep1) {
      setState(() => _returnToReviewAfterStep1 = false);
      _goToPage(3);
    } else {
      _nextPage();
    }
  }

  void _handleStepContinue() {
    switch (_currentIndex) {
      case 1:
        if (_step2FormKey.currentState!.validate()) {
          if (_returnToReviewAfterStep2) {
            setState(() => _returnToReviewAfterStep2 = false);
            _goToPage(3);
          } else {
            _nextPage();
          }
        }
        break;
      case 2:
        if (_step3FormKey.currentState!.validate()) _nextPage();
        break;
    }
  }

  void _handleCreateAccount() {
    if (_selectedDateOfBirth == null || _selectedGender == null) return;

    final weightText = _weightController.text.trim();
    final heightText = _heightController.text.trim();
    final emergencyNameText = _emergencyNameController.text.trim();
    final emergencyPhoneText = _emergencyPhoneController.text.trim();

    final requestModel = RegisterPatientRequestModel(
      email: _emailController.text.trim(),
      phoneNumber: _phoneController.text.trim(),
      password: _passwordController.text,
      fullName: _fullNameController.text.trim(),
      nationalId: _nationalIdController.text.trim(),
      dateOfBirth: DateFormatterHelper.formatForApi(_selectedDateOfBirth!),
      gender: _selectedGender!,
      bloodType: BloodTypeHelper.toApiValue(_selectedBloodType),
      weightKg: weightText.isEmpty ? null : double.tryParse(weightText),
      heightCm: heightText.isEmpty ? null : double.tryParse(heightText),
      emergencyContactName: emergencyNameText.isEmpty
          ? null
          : emergencyNameText,
      emergencyContactPhone: emergencyPhoneText.isEmpty
          ? null
          : emergencyPhoneText,
    );

    context.read<RegisterPatientCubit>().registerPatient(requestModel);
  }

  (String title, String subtitle) _getHeaderText(S s) {
    switch (_currentIndex) {
      case 0:
        return (s.createAccountTitle, s.accountDetailsSubtitle);
      case 1:
        return (s.personalDetailsTitle, s.personalDetailsSubtitle);
      case 2:
        return (s.clinicalBasicsTitle, s.clinicalBasicsSubtitle);
      case 3:
      default:
        return (s.emergencyContactTitle, s.emergencyContactSubtitle);
    }
  }

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);
    final (titleText, subTitleText) = _getHeaderText(s);

    return BlocConsumer<RegisterPatientCubit, AuthState>(
      listener: (context, state) {
        state.whenOrNull(
          success: (entity) {
            context.pushNamedAndRemoveUntil(
              Routes.homeScreen,
              predicate: (_) => false,
            );
          },
          failure: (error) {
            final lowerError = error.toLowerCase();
            final isEmailError = lowerError.contains('email');
            final isPhoneError =
                lowerError.contains('phone') || lowerError.contains('mobile');
            final isNationalIdError =
                lowerError.contains('national') || lowerError.contains('id');

            if (isEmailError || isPhoneError) {
              setState(() {
                _serverEmailError = isEmailError ? error : null;
                _serverPhoneError = isPhoneError ? error : null;
                _returnToReviewAfterStep1 = true;
              });
              _goToPage(0);
            } else if (isNationalIdError) {
              setState(() {
                _serverNationalIdError = error;
                _returnToReviewAfterStep2 = true;
              });
              _goToPage(1);
            } else {
              ScaffoldMessenger.of(context)
                ..hideCurrentSnackBar()
                ..showSnackBar(
                  SnackBar(
                    content: Text(error),
                    backgroundColor: AppColors.error,
                  ),
                );
            }
          },
        );
      },
      builder: (context, state) {
        final isLoading = state is AuthLoading;

        return PopScope(
          canPop: _currentIndex == 0 && !isLoading,
          onPopInvokedWithResult: (didPop, _) {
            if (!didPop && _currentIndex > 0 && !isLoading) {
              _previousPage();
            }
          },
          child: AnnotatedRegion<SystemUiOverlayStyle>(
            value: SystemUiOverlayStyle.dark,
            child: Scaffold(
              body: SafeArea(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    SizedBox(height: AppDimensions.spacing12.h),
                    Padding(
                      padding: EdgeInsetsDirectional.symmetric(
                        horizontal: AppDimensions.spacing24.w,
                      ),
                      child: RegisterHeader(
                        currentIndex: _currentIndex,
                        titleText: titleText,
                        subTitleText: subTitleText,
                      ),
                    ),
                    Expanded(
                      child: PageView(
                        physics: const NeverScrollableScrollPhysics(),
                        controller: _pageController,
                        onPageChanged: (index) {
                          setState(() {
                            _currentIndex = index;
                          });
                        },
                        children: [
                          RegisterStepOneAccount(
                            formKey: _step1FormKey,
                            emailController: _emailController,
                            phoneController: _phoneController,
                            passwordController: _passwordController,
                            confirmPasswordController:
                                _confirmPasswordController,
                            serverEmailError: _serverEmailError,
                            serverPhoneError: _serverPhoneError,
                            isEditingFromReview: _returnToReviewAfterStep1,
                            onEmailChanged: (_) {
                              if (_serverEmailError != null) {
                                setState(() => _serverEmailError = null);
                              }
                            },
                            onPhoneChanged: (_) {
                              if (_serverPhoneError != null) {
                                setState(() => _serverPhoneError = null);
                              }
                            },
                            onContinue: _handleStep1Continue,
                          ),
                          RegisterStepTwoPersonal(
                            formKey: _step2FormKey,
                            fullNameController: _fullNameController,
                            nationalIdController: _nationalIdController,
                            selectedDateOfBirth: _selectedDateOfBirth,
                            onDateOfBirthChanged: (date) {
                              setState(() => _selectedDateOfBirth = date);
                            },
                            selectedGender: _selectedGender,
                            onGenderChanged: (gender) {
                              setState(() => _selectedGender = gender);
                            },
                            serverNationalIdError: _serverNationalIdError,
                            onNationalIdChanged: (_) {
                              if (_serverNationalIdError != null) {
                                setState(() => _serverNationalIdError = null);
                              }
                            },
                          ),
                          RegisterStepThreeClinical(
                            formKey: _step3FormKey,
                            selectedBloodType: _selectedBloodType,
                            onBloodTypeChanged: (type) {
                              setState(() => _selectedBloodType = type);
                            },
                            weightController: _weightController,
                            heightController: _heightController,
                          ),
                          RegisterStepFourEmergency(
                            formKey: _step4FormKey,
                            emergencyNameController: _emergencyNameController,
                            emergencyPhoneController: _emergencyPhoneController,
                            email: _emailController.text,
                            phone: _phoneController.text,
                            fullName: _fullNameController.text,
                            nationalId: _nationalIdController.text,
                            dateOfBirth: _selectedDateOfBirth,
                            gender: _selectedGender,
                            bloodType: _selectedBloodType,
                            weight: _weightController.text,
                            height: _heightController.text,
                            onEditStep: _goToPage,
                            onCreateAccount: _handleCreateAccount,
                            onBack: _previousPage,
                            isLoading: isLoading,
                          ),
                        ],
                      ),
                    ),
                    if (_currentIndex == 1 || _currentIndex == 2)
                      Padding(
                        padding: EdgeInsetsDirectional.fromSTEB(
                          AppDimensions.spacing24.w,
                          AppDimensions.spacing12.h,
                          AppDimensions.spacing24.w,
                          AppDimensions.spacing24.h,
                        ),
                        child: RegisterNavigationButtons(
                          onBack: _previousPage,
                          onContinue: _handleStepContinue,
                          continueText:
                              (_currentIndex == 1 && _returnToReviewAfterStep2)
                              ? s.continueToReviewButton
                              : s.continueButton,
                        ),
                      ),
                  ],
                ),
              ),
            ),
          ),
        );
      },
    );
  }
}
