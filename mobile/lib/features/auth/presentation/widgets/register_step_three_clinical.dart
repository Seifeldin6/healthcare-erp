import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/helpers/blood_type_helper.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/core/widgets/app_primary_button.dart';
import 'package:healthcare_app/core/widgets/app_text_form_field.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/auth_info_notice.dart';
import 'package:healthcare_app/generated/l10n.dart';

class RegisterStepThreeClinical extends StatelessWidget {
  final GlobalKey<FormState> formKey;
  final String? selectedBloodType;
  final ValueChanged<String?> onBloodTypeChanged;
  final TextEditingController weightController;
  final TextEditingController heightController;

  const RegisterStepThreeClinical({
    super.key,
    required this.formKey,
    required this.selectedBloodType,
    required this.onBloodTypeChanged,
    required this.weightController,
    required this.heightController,
  });

  Widget _buildBloodOption({
    required String label,
    required String value,
    double fontSize = 15,
  }) {
    final isSelected = selectedBloodType == value;
    return AppPrimaryButton(
      text: label,
      backgroundColor: isSelected ? AppColors.primaryTint : AppColors.surface,
      textStyle:
          (isSelected
                  ? AppTextStyles.body14PrimaryBold
                  : AppTextStyles.label14InkSemiBold)
              .copyWith(fontSize: fontSize.sp),
      borderSide: BorderSide(
        color: isSelected ? AppColors.primary : AppColors.line,
        width: isSelected ? 1.5 : 1,
      ),
      onPressed: () => onBloodTypeChanged(value),
    );
  }

  Widget _buildBloodTypeRow(List<String> types) {
    return Row(
      children: [
        for (int i = 0; i < types.length; i++) ...[
          Expanded(
            child: _buildBloodOption(label: types[i], value: types[i]),
          ),
          if (i < types.length - 1) SizedBox(width: 8.w),
        ],
      ],
    );
  }

  Widget _buildUnitBadge(String unit) {
    return Column(
      mainAxisAlignment: MainAxisAlignment.center,
      mainAxisSize: MainAxisSize.min,
      children: [
        Container(
          margin: EdgeInsetsDirectional.only(end: 12.w),
          padding: EdgeInsets.symmetric(horizontal: 10.w, vertical: 4.h),
          decoration: BoxDecoration(
            color: AppColors.noticeBg,
            borderRadius: BorderRadius.circular(8.r),
          ),
          child: Text(
            unit,
            style: AppTextStyles.caption12MutedRegular.copyWith(
              fontWeight: FontWeight.w600,
            ),
          ),
        ),
      ],
    );
  }

  String? _validatePositiveNumber(String? value, String errorMessage) {
    if (value != null && value.trim().isNotEmpty) {
      final parsed = double.tryParse(value.trim());
      if (parsed == null || parsed <= 0) {
        return errorMessage;
      }
    }
    return null;
  }

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

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
            // 1. Blood Type Section Header
            Row(
              children: [
                Icon(
                  Icons.water_drop_outlined,
                  color: AppColors.error,
                  size: 18.sp,
                ),
                SizedBox(width: 6.w),
                Text(s.bloodTypeLabel, style: AppTextStyles.label14InkSemiBold),
              ],
            ),
            SizedBox(height: AppDimensions.spacing12.h),

            // 2. Blood Type Grid
            Directionality(
              textDirection: TextDirection.ltr,
              child: Column(
                children: [
                  _buildBloodTypeRow(BloodTypeHelper.firstRowTypes),
                  SizedBox(height: 10.h),
                  _buildBloodTypeRow(BloodTypeHelper.secondRowTypes),
                ],
              ),
            ),
            SizedBox(height: 10.h),

            // 3. Unknown Blood Type Option
            SizedBox(
              width: double.infinity,
              child: _buildBloodOption(
                label: s.dontKnowBloodType,
                value: BloodTypeHelper.unknown,
                fontSize: 14,
              ),
            ),
            SizedBox(height: 20.h),

            // 4. Weight & Height Row
            Row(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Expanded(
                  child: AppTextFormField(
                    controller: weightController,
                    label: s.weightLabel,
                    hintText: s.weightHint,
                    textDirection: TextDirection.ltr,
                    keyboardType: const TextInputType.numberWithOptions(
                      decimal: true,
                    ),
                    textInputAction: TextInputAction.next,
                    suffixIcon: _buildUnitBadge(s.kgUnit),
                    validator: (value) =>
                        _validatePositiveNumber(value, s.invalidNumberError),
                  ),
                ),
                SizedBox(width: AppDimensions.spacing12.w),
                Expanded(
                  child: AppTextFormField(
                    controller: heightController,
                    label: s.heightLabel,
                    hintText: s.heightHint,
                    textDirection: TextDirection.ltr,
                    keyboardType: const TextInputType.numberWithOptions(
                      decimal: true,
                    ),
                    textInputAction: TextInputAction.done,
                    suffixIcon: _buildUnitBadge(s.cmUnit),
                    validator: (value) =>
                        _validatePositiveNumber(value, s.invalidNumberError),
                  ),
                ),
              ],
            ),
            SizedBox(height: 20.h),

            // 5. Clinical Info Banner
            AuthInfoNotice(message: s.clinicalInfoBanner),
          ],
        ),
      ),
    );
  }
}
