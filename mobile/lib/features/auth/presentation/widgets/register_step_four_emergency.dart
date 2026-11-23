import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/helpers/app_regex.dart';
import 'package:healthcare_app/core/helpers/blood_type_helper.dart';
import 'package:healthcare_app/core/helpers/date_formatter_helper.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/core/widgets/app_primary_button.dart';
import 'package:healthcare_app/core/widgets/app_text_form_field.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/register_review_section_card.dart';
import 'package:healthcare_app/generated/l10n.dart';

class RegisterStepFourEmergency extends StatelessWidget {
  final GlobalKey<FormState> formKey;
  final TextEditingController emergencyNameController;
  final TextEditingController emergencyPhoneController;

  // Review Data
  final String email;
  final String phone;
  final String fullName;
  final String nationalId;
  final DateTime? dateOfBirth;
  final String? gender;
  final String? bloodType;
  final String weight;
  final String height;

  // Actions
  final ValueChanged<int> onEditStep;
  final VoidCallback onCreateAccount;
  final VoidCallback onBack;
  final bool isLoading;

  const RegisterStepFourEmergency({
    super.key,
    required this.formKey,
    required this.emergencyNameController,
    required this.emergencyPhoneController,
    required this.email,
    required this.phone,
    required this.fullName,
    required this.nationalId,
    required this.dateOfBirth,
    required this.gender,
    required this.bloodType,
    required this.weight,
    required this.height,
    required this.onEditStep,
    required this.onCreateAccount,
    required this.onBack,
    this.isLoading = false,
  });

  String _maskNationalId(String id, String fallback) {
    final trimmed = id.trim();
    if (trimmed.isEmpty) return fallback;
    if (trimmed.length <= 8) return trimmed;
    final start = trimmed.substring(0, 6);
    final end = trimmed.substring(trimmed.length - 2);
    return '$start••••••$end';
  }

  Widget _buildKeyValueRow(
    String label,
    String value, {
    bool isLast = false,
    TextDirection? valueDirection,
  }) {
    return Padding(
      padding: EdgeInsets.only(bottom: isLast ? 0 : 10.h),
      child: Row(
        children: [
          Expanded(
            flex: 4,
            child: Text(label, style: AppTextStyles.label14MutedRegular),
          ),
          Expanded(
            flex: 6,
            child: Text(
              value,
              maxLines: 1,
              overflow: TextOverflow.ellipsis,
              textDirection: valueDirection,
              style: AppTextStyles.label14InkSemiBold,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildClinicalMetricBox({
    required String label,
    required String value,
    String? unit,
  }) {
    final hasValue = value != '—';

    return Expanded(
      child: Container(
        padding: EdgeInsets.symmetric(horizontal: 10.w, vertical: 10.h),
        decoration: BoxDecoration(
          color: AppColors.noticeBg,
          borderRadius: BorderRadius.circular(12.r),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              label,
              maxLines: 1,
              overflow: TextOverflow.ellipsis,
              style: AppTextStyles.caption12MutedRegular,
            ),
            SizedBox(height: 4.h),
            Row(
              crossAxisAlignment: CrossAxisAlignment.baseline,
              textBaseline: TextBaseline.alphabetic,
              children: [
                Flexible(
                  child: Text(
                    value,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    textDirection: TextDirection.ltr,
                    style: AppTextStyles.body16InkRegular.copyWith(
                      fontWeight: FontWeight.w700,
                    ),
                  ),
                ),
                if (unit != null && hasValue) ...[
                  SizedBox(width: 4.w),
                  Text(unit, style: AppTextStyles.caption12MutedRegular),
                ],
              ],
            ),
          ],
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);
    final isAr = Localizations.localeOf(context).languageCode == 'ar';

    final displayGender = gender == null
        ? s.notProvided
        : (gender == 'Male' ? s.maleOption : s.femaleOption);

    final displayBloodType =
        (bloodType == null || bloodType == BloodTypeHelper.unknown)
        ? '—'
        : bloodType!;

    return SingleChildScrollView(
      padding: EdgeInsetsDirectional.fromSTEB(
        AppDimensions.spacing24.w,
        AppDimensions.spacing16.h,
        AppDimensions.spacing24.w,
        AppDimensions.spacing24.h,
      ),
      child: Form(
        key: formKey,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // 1. Contact Name
            AppTextFormField(
              controller: emergencyNameController,
              label: s.emergencyContactNameLabel,
              hintText: s.emergencyContactNameHint,
              keyboardType: TextInputType.name,
              textInputAction: TextInputAction.next,
              prefixIcon: Icon(
                Icons.person_outline_rounded,
                color: AppColors.muted,
                size: 20.sp,
              ),
            ),
            SizedBox(height: AppDimensions.spacing16.h),
            // 2. Contact Phone
            AppTextFormField(
              controller: emergencyPhoneController,
              label: s.emergencyContactPhoneLabel,
              hintText: s.emergencyContactPhoneHint,
              textDirection: TextDirection.ltr,
              keyboardType: TextInputType.phone,
              textInputAction: TextInputAction.done,
              prefixIcon: Icon(
                Icons.phone_outlined,
                color: AppColors.muted,
                size: 20.sp,
              ),
              validator: (value) {
                if (value != null &&
                    value.trim().isNotEmpty &&
                    !AppRegex.isPhoneNumberValid(value.trim())) {
                  return s.invalidPhoneError;
                }
                return null;
              },
            ),
            SizedBox(height: AppDimensions.spacing24.h),

            // 3. Review Your Details Header
            Row(
              crossAxisAlignment: CrossAxisAlignment.baseline,
              textBaseline: TextBaseline.alphabetic,
              children: [
                Expanded(
                  child: Text(
                    s.reviewYourDetailsTitle,
                    style: AppTextStyles.body16InkRegular.copyWith(
                      fontWeight: FontWeight.w700,
                    ),
                  ),
                ),
                SizedBox(width: 8.w),
                Text(
                  s.tapEditToChangeStep,
                  style: AppTextStyles.caption12MutedRegular,
                ),
              ],
            ),
            SizedBox(height: AppDimensions.spacing12.h),

            // 4. Card 1: Account
            RegisterReviewSectionCard(
              title: s.accountSectionTitle,
              onEdit: () => onEditStep(0),
              child: Column(
                children: [
                  _buildKeyValueRow(
                    s.emailLabel,
                    email.trim().isEmpty ? s.notProvided : email.trim(),
                    valueDirection: TextDirection.ltr,
                  ),
                  _buildKeyValueRow(
                    s.phoneNumberLabel,
                    phone.trim().isEmpty ? s.notProvided : phone.trim(),
                    valueDirection: TextDirection.ltr,
                  ),
                  _buildKeyValueRow(
                    s.passwordLabel,
                    '••••••••',
                    isLast: true,
                    valueDirection: TextDirection.ltr,
                  ),
                ],
              ),
            ),
            SizedBox(height: AppDimensions.spacing12.h),

            // 5. Card 2: Personal
            RegisterReviewSectionCard(
              title: s.personalSectionTitle,
              onEdit: () => onEditStep(1),
              child: Column(
                children: [
                  _buildKeyValueRow(
                    s.fullNameLabel,
                    fullName.trim().isEmpty ? s.notProvided : fullName.trim(),
                  ),
                  _buildKeyValueRow(
                    s.nationalIdLabel,
                    _maskNationalId(nationalId, s.notProvided),
                    valueDirection: TextDirection.ltr,
                  ),
                  _buildKeyValueRow(
                    s.dateOfBirthLabel,
                    DateFormatterHelper.formatReadableDate(
                      dateOfBirth,
                      isArabic: isAr,
                      fallback: s.notProvided,
                    ),
                  ),
                  _buildKeyValueRow(s.genderLabel, displayGender, isLast: true),
                ],
              ),
            ),
            SizedBox(height: AppDimensions.spacing12.h),

            // 6. Card 3: Clinical
            RegisterReviewSectionCard(
              title: s.clinicalSectionTitle,
              onEdit: () => onEditStep(2),
              child: Row(
                children: [
                  _buildClinicalMetricBox(
                    label: s.bloodLabel,
                    value: displayBloodType,
                  ),
                  SizedBox(width: 8.w),
                  _buildClinicalMetricBox(
                    label: s.weightLabel,
                    value: weight.trim().isEmpty ? '—' : weight.trim(),
                    unit: s.kgUnit,
                  ),
                  SizedBox(width: 8.w),
                  _buildClinicalMetricBox(
                    label: s.heightLabel,
                    value: height.trim().isEmpty ? '—' : height.trim(),
                    unit: s.cmUnit,
                  ),
                ],
              ),
            ),
            SizedBox(height: AppDimensions.spacing24.h),

            // 7. Create Account & Back Buttons
            SizedBox(
              width: double.infinity,
              child: AppPrimaryButton(
                text: s.createAccountButton,
                isLoading: isLoading,
                onPressed: () {
                  if (formKey.currentState!.validate()) {
                    onCreateAccount();
                  }
                },
              ),
            ),
            SizedBox(height: 8.h),
            SizedBox(
              width: double.infinity,
              child: AppPrimaryButton(
                text: s.backButton,
                backgroundColor: Colors.transparent,
                textStyle: AppTextStyles.label14MutedRegular.copyWith(
                  fontWeight: FontWeight.w600,
                ),
                onPressed: isLoading ? null : onBack,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
