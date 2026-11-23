import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/helpers/app_regex.dart';
import 'package:healthcare_app/core/helpers/date_formatter_helper.dart';
import 'package:healthcare_app/core/helpers/extensions.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/core/widgets/app_labeled_form_field.dart';
import 'package:healthcare_app/core/widgets/app_primary_button.dart';
import 'package:healthcare_app/core/widgets/app_text_form_field.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/login_error_banner.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/server_field_error_text.dart';
import 'package:healthcare_app/generated/l10n.dart';

class RegisterStepTwoPersonal extends StatelessWidget {
  final GlobalKey<FormState> formKey;
  final TextEditingController fullNameController;
  final TextEditingController nationalIdController;
  final DateTime? selectedDateOfBirth;
  final ValueChanged<DateTime> onDateOfBirthChanged;
  final String? selectedGender;
  final ValueChanged<String> onGenderChanged;
  final String? serverNationalIdError;
  final ValueChanged<String>? onNationalIdChanged;

  const RegisterStepTwoPersonal({
    super.key,
    required this.formKey,
    required this.fullNameController,
    required this.nationalIdController,
    required this.selectedDateOfBirth,
    required this.onDateOfBirthChanged,
    required this.selectedGender,
    required this.onGenderChanged,
    this.serverNationalIdError,
    this.onNationalIdChanged,
  });

  Future<void> _pickDateOfBirth(
    BuildContext context,
    FormFieldState<DateTime> fieldState,
  ) async {
    final now = DateTime.now();
    final initialDate =
        selectedDateOfBirth ?? DateTime(now.year - 25, now.month, now.day);

    final picked = await showDatePicker(
      context: context,
      initialDate: initialDate,
      firstDate: DateTime(1940),
      lastDate: now,
      builder: (context, child) {
        return Theme(
          data: Theme.of(context).copyWith(
            colorScheme: Theme.of(
              context,
            ).colorScheme.copyWith(primary: AppColors.primary),
          ),
          child: child!,
        );
      },
    );

    if (picked != null) {
      onDateOfBirthChanged(picked);
      fieldState.didChange(picked);
    }
  }

  Widget _buildGenderOption({
    required String label,
    required String value,
    required bool hasError,
    required FormFieldState<String> fieldState,
  }) {
    final isSelected = selectedGender == value;

    return Expanded(
      child: AppPrimaryButton(
        text: label,
        prefixIcon: isSelected ? Icons.check_rounded : null,
        backgroundColor: isSelected ? AppColors.primaryTint : AppColors.surface,
        textStyle:
            (isSelected
                    ? AppTextStyles.body14PrimaryBold
                    : AppTextStyles.label14InkSemiBold)
                .copyWith(fontSize: 15.sp),
        borderSide: BorderSide(
          color: isSelected
              ? AppColors.primary
              : (hasError ? AppColors.error : AppColors.line),
          width: isSelected ? 1.5 : 1,
        ),
        onPressed: () {
          onGenderChanged(value);
          fieldState.didChange(value);
        },
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);
    final inputTheme = Theme.of(context).inputDecorationTheme;
    final hasNationalIdError = !serverNationalIdError.isNullOrEmpty();

    return SingleChildScrollView(
      padding: EdgeInsetsDirectional.symmetric(
        horizontal: AppDimensions.spacing24.w,
        vertical: AppDimensions.spacing16.h,
      ),
      child: Form(
        key: formKey,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            if (hasNationalIdError) ...[
              LoginErrorBanner(
                title: s.couldntCreateAccountTitle,
                message: s.couldntCreateAccountSubtitle,
              ),
              SizedBox(height: AppDimensions.spacing16.h),
            ],

            // 1. Full Name
            AppTextFormField(
              controller: fullNameController,
              label: s.fullNameLabel,
              hintText: s.fullNameHint,
              keyboardType: TextInputType.name,
              textInputAction: TextInputAction.next,
              prefixIcon: Icon(
                Icons.person_outline_rounded,
                color: AppColors.muted,
                size: 20.sp,
              ),
              validator: (value) {
                if (value == null || value.trim().isEmpty) {
                  return s.fullNameRequiredError;
                }
                return null;
              },
            ),
            SizedBox(height: AppDimensions.spacing16.h),

            // 2. National ID
            AppTextFormField(
              controller: nationalIdController,
              label: s.nationalIdLabel,
              hintText: s.nationalIdHint,
              textDirection: TextDirection.ltr,
              keyboardType: TextInputType.number,
              textInputAction: TextInputAction.done,
              hasError: hasNationalIdError,
              onChanged: onNationalIdChanged,
              prefixIcon: Icon(
                Icons.badge_outlined,
                color: hasNationalIdError ? AppColors.error : AppColors.muted,
                size: 20.sp,
              ),
              validator: (value) {
                if (value == null || value.trim().isEmpty) {
                  return s.nationalIdRequiredError;
                }
                if (!AppRegex.isNationalIdValid(value.trim())) {
                  return s.invalidNationalIdError;
                }
                return null;
              },
            ),
            if (hasNationalIdError)
              ServerFieldErrorText(errorMessage: serverNationalIdError!),
            SizedBox(height: AppDimensions.spacing16.h),

            // 3. Date of Birth (Generic FormField)
            AppLabeledFormField<DateTime>(
              label: s.dateOfBirthLabel,
              initialValue: selectedDateOfBirth,
              validator: (_) => selectedDateOfBirth == null
                  ? s.dateOfBirthRequiredError
                  : null,
              builder: (fieldState) {
                return InkWell(
                  onTap: () => _pickDateOfBirth(context, fieldState),
                  borderRadius: BorderRadius.circular(
                    AppDimensions.cardRadius.r,
                  ),
                  child: InputDecorator(
                    decoration: InputDecoration(
                      prefixIcon: Icon(
                        Icons.calendar_today_outlined,
                        color: AppColors.muted,
                        size: 20.sp,
                      ),
                      suffixIcon: Icon(
                        Icons.keyboard_arrow_down_rounded,
                        color: AppColors.muted,
                        size: 22.sp,
                      ),
                      enabledBorder: fieldState.hasError
                          ? inputTheme.errorBorder
                          : null,
                    ),
                    child: Text(
                      selectedDateOfBirth != null
                          ? DateFormatterHelper.formatNumericDate(
                              selectedDateOfBirth!,
                            )
                          : s.dateOfBirthHint,
                      textDirection: TextDirection.ltr,
                      style: selectedDateOfBirth != null
                          ? AppTextStyles.body16InkRegular
                          : AppTextStyles.label14MutedRegular,
                    ),
                  ),
                );
              },
            ),
            SizedBox(height: AppDimensions.spacing16.h),

            // 4. Gender (Generic FormField)
            AppLabeledFormField<String>(
              label: s.genderLabel,
              initialValue: selectedGender,
              validator: (_) =>
                  selectedGender == null ? s.genderRequiredError : null,
              builder: (fieldState) {
                return Row(
                  children: [
                    _buildGenderOption(
                      label: s.maleOption,
                      value: 'Male',
                      hasError: fieldState.hasError,
                      fieldState: fieldState,
                    ),
                    SizedBox(width: AppDimensions.spacing12.w),
                    _buildGenderOption(
                      label: s.femaleOption,
                      value: 'Female',
                      hasError: fieldState.hasError,
                      fieldState: fieldState,
                    ),
                  ],
                );
              },
            ),
          ],
        ),
      ),
    );
  }
}
